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
    /// <summary>
    /// Singleton DataSource object.  Contains a reference to broadlyEntities object which is lazily instantiated,
    /// although that's not much of an issue with this app. 
    /// </summary>
    public class DataSource
    {


        private static DataSource instance;

        private static broadlyEntities db_entities;

        static DataSource()
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
        public IEnumerable<Transaction> GetTransactions(DateTime SearchDate)
        {
            if (db_entities == null)
            {
                db_entities = new broadlyEntities();
            }

            return GetTransactions(SearchDate, db_entities);
        }

        /// <summary>
        /// Get Transactions from a search date and our database.  Public for the purposes of unit testing. 
        /// </summary>
        /// <param name="SearchDate">The date to look for completed transactions.</param>
        /// <param name="DbEntities">The representation of the database.</param>
        /// <returns></returns>
        public static IEnumerable<Transaction> GetTransactions(DateTime SearchDate, broadlyEntities DbEntities)
        {
            IEnumerable<Transaction> load = null;
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
