using Microsoft.VisualStudio.TestTools.UnitTesting;
using BroadlyDatabaseToJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using System.Data.Entity;

namespace BroadlyDatabaseToJson.Tests
{
    [TestClass()]
    public class DataSourceTests
    {
        [TestMethod()]
        public void GetDataOrdersTest()
        {
            List<appointment_data> appts = GetFakeAppointments();
            List<patient_data> patients = GetFakePatients();

            //var mock_appts = new Mock<DbSet<appointment_data>>();
            //mock_appts.As<IQueryable<appointment_data>>().Setup(m => m.Provider)
            //    .Returns(appts.AsQueryable<appointment_data>().Provider);
            //mock_appts.As<IQueryable<appointment_data>>().Setup(m => m.Expression)
            //    .Returns(appts.AsQueryable<appointment_data>().Expression);
            //mock_appts.As<IQueryable<appointment_data>>().Setup(m => m.ElementType)
            //    .Returns(appts.AsQueryable<appointment_data>().ElementType);
            //mock_appts.As<IQueryable<appointment_data>>().Setup(m => m.GetEnumerator())
            //    .Returns(appts.AsQueryable<appointment_data>().GetEnumerator());

            //var mock_patients = new Mock<DbSet<patient_data>>();
            //mock_patients.As<IQueryable<patient_data>>().Setup(m => m.Provider)
            //    .Returns(patients.AsQueryable<patient_data>().Provider);
            //mock_patients.As<IQueryable<patient_data>>().Setup(m => m.Expression)
            //    .Returns(patients.AsQueryable<patient_data>().Expression);
            //mock_patients.As<IQueryable<patient_data>>().Setup(m => m.ElementType)
            //    .Returns(patients.AsQueryable<patient_data>().ElementType);
            //mock_patients.As<IQueryable<patient_data>>().Setup(m => m.GetEnumerator())
            //    .Returns(patients.AsQueryable<patient_data>().GetEnumerator());


            var fake_entities = new Mock<broadlyEntities>();
            //fake_entities.Setup(c => c.appointment_data).Returns(mock_appts.Object);
            //fake_entities.Setup(c => c.patient_data).Returns(mock_patients.Object);

            DateTime compare_date = new DateTime(2016, 1, 26);

            DataSource source = DataSource.Instance;
            var orders = source.GetDataOrders(compare_date, fake_entities.Object);

            Assert.IsNotNull(orders);
            Assert.IsTrue(orders is IQueryable);
        }

        private static List<appointment_data> GetFakeAppointments()
        {
            List<appointment_data> fake_appointments = new List<appointment_data>();
            appointment_data appt = new appointment_data();
            appt.appointment_id = 1;
            appt.patient_id = 1;
            appt.scheduled = new DateTime(2016, 1, 26);
            appt.status = "completed";
            appt.updatedAt = new DateTime(2016, 1, 26);
            fake_appointments.Add(appt);

            appt = new appointment_data();
            appt.appointment_id = 2;
            appt.patient_id = 2;
            appt.scheduled = new DateTime(2016, 1, 26);
            appt.status = "canceled";
            appt.updatedAt = new DateTime(2016, 1, 26);
            fake_appointments.Add(appt);

            appt = new appointment_data();
            appt.appointment_id = 3;
            appt.patient_id = 1;
            appt.scheduled = new DateTime(2016, 1, 27);
            appt.status = "completed";
            appt.updatedAt = new DateTime(2016, 1, 25);
            fake_appointments.Add(appt);

            return fake_appointments;
        }

        private static List<patient_data> GetFakePatients()
        {
            List<patient_data> patients = new List<patient_data>();
            patient_data patient = new patient_data();
            patient.age = 25;
            patient.email = "abc@xyz.com";
            patient.first_name = "FirstName";
            patient.last_name = "LastName";
            patient.patient_id = 1;
            patient.phone = "510-555-1234";
            patients.Add(patient);

            patient = new patient_data();
            patient.age = 33;
            patient.email = "def@xyz.com";
            patient.first_name = "FirstName2";
            patient.last_name = "LastName2";
            patient.patient_id = 2;
            patient.phone = "510-555-5678";
            patients.Add(patient);

            return patients;
        }
    }
}