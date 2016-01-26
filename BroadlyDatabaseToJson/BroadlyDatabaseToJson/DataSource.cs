using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;


// addded these
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Entity;

namespace BroadlyDatabaseToJson
{
    public class DataSource
    {

        private static DataSource instance;

        private static broadlyEntities db_entities;

        private DataSource()
        {
            
        }

        public static DataSource Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DataSource();
                }
                return instance;
            }
        }

        /// <summary>
        /// Get data order objects.
        /// </summary>
        /// <param name="SearchDate"></param>
        /// <returns></returns>
        public IEnumerable GetDataOrders(DateTime SearchDate)
        {
            if (db_entities == null)
            {
                db_entities = new broadlyEntities();
            }

            return GetDataOrders(SearchDate, db_entities);
        }

        /// <summary>
        /// Get data order objects.
        /// </summary>
        /// <param name="SearchDate"></param>
        /// <param name="DbEntities"></param>
        /// <returns></returns>
        public IEnumerable GetDataOrders(DateTime SearchDate, broadlyEntities DbEntities)
        {
            IEnumerable load = null;
            try
            {
                // This can be written in one string I think. 
                IQueryable<appointment_data> appt_data =
                    from appointment in DbEntities.appointment_data
                    where appointment.status == "completed" &&
                        appointment.scheduled.HasValue &&
                        // ugly date filter because LINQ doesn't like comparing DateTimes...
                        appointment.scheduled.Value.Year == SearchDate.Year &&
                        appointment.scheduled.Value.Day == SearchDate.Day &&
                        appointment.scheduled.Value.Month == SearchDate.Month &&
                        appointment.updatedAt.HasValue 
                    select appointment;

                load =
                    from patient in DbEntities.patient_data
                    where !string.IsNullOrEmpty(patient.email)
                    join appointment in appt_data on patient.patient_id equals appointment.patient_id
                    select new Transaction()  {
                        AppointmentId = appointment.appointment_id,
                        AppointmentTime = appointment.scheduled,
                        PatientId = patient.patient_id,
                        PatientFirstName = patient.first_name,
                        PatientLastName = patient.last_name,
                        PatientEmail = patient.email,
                        PatientPhone = patient.phone};
            }
            catch (Exception e)
            {
                // some sort of error reporting
                Console.WriteLine(e.Message);
            }

            return load;
        }
    }
}
