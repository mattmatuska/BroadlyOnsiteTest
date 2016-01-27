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
        /// <summary>
        /// Test some basic lists.
        /// </summary>
        [TestMethod()]
        public void GetDataOrdersTest()
        {
            List<appointment_data> appts = new List<appointment_data>();
            appts.Add(Appointment0);
            appts.Add(Appointment1);
            appts.Add(Appointment2);

            List<patient_data> patients = new List<patient_data>();
            patients.Add(Patient0);
            patients.Add(Patient1);

            DateTime compare_date = new DateTime(2016, 1, 26);

            IEnumerable<Transaction> transactions = TestPatientsAndAppointments(appts, patients, compare_date);
            Assert.IsTrue(transactions.Count(x => true) == 1);
        }

        /// <summary>
        /// Explicitly test lack of email.
        /// </summary>
        [TestMethod()]
        public void GetDataOrdersTestNoEmail()
        {
            List<appointment_data> appts = new List<appointment_data>();
            appts.Add(Appointment0);
            appts.Add(Appointment1);
            appts.Add(Appointment2);

            List<patient_data> patients = new List<patient_data>();
            patients.Add(Patient0NoEmail);
            patients.Add(Patient1);

            DateTime compare_date = new DateTime(2016, 1, 26);

            // no transactions, should be one less.
            IEnumerable<Transaction> transactions = TestPatientsAndAppointments(appts, patients, compare_date);
            Assert.IsTrue(transactions.Count(x => true) == 0);
        }

        /// <summary>
        /// Tests patient and appointment lists.
        /// </summary>
        /// <param name="appts">List of appointments to substitute in our mockup.</param>
        /// <param name="patients">List of patients to substitute in our mockup.</param>
        /// <param name="compare_date">The date to compare in our mockup.</param>
        /// <returns></returns>
        private static IEnumerable<Transaction> TestPatientsAndAppointments(
            List<appointment_data> appts, 
            List<patient_data> patients, 
            DateTime compare_date)
        {
            var mock_appts = new Mock<DbSet<appointment_data>>();
            mock_appts.As<IQueryable<appointment_data>>().Setup(m => m.Provider)
                .Returns(appts.AsQueryable<appointment_data>().Provider);
            mock_appts.As<IQueryable<appointment_data>>().Setup(m => m.Expression)
                .Returns(appts.AsQueryable<appointment_data>().Expression);
            mock_appts.As<IQueryable<appointment_data>>().Setup(m => m.ElementType)
                .Returns(appts.AsQueryable<appointment_data>().ElementType);
            mock_appts.As<IQueryable<appointment_data>>().Setup(m => m.GetEnumerator())
                .Returns(appts.AsQueryable<appointment_data>().GetEnumerator());

            var mock_patients = new Mock<DbSet<patient_data>>();
            mock_patients.As<IQueryable<patient_data>>().Setup(m => m.Provider)
                .Returns(patients.AsQueryable<patient_data>().Provider);
            mock_patients.As<IQueryable<patient_data>>().Setup(m => m.Expression)
                .Returns(patients.AsQueryable<patient_data>().Expression);
            mock_patients.As<IQueryable<patient_data>>().Setup(m => m.ElementType)
                .Returns(patients.AsQueryable<patient_data>().ElementType);
            mock_patients.As<IQueryable<patient_data>>().Setup(m => m.GetEnumerator())
                .Returns(patients.AsQueryable<patient_data>().GetEnumerator());


            var fake_entities = new Mock<broadlyEntities>();
            fake_entities.Setup(c => c.appointment_data).Returns(mock_appts.Object);
            fake_entities.Setup(c => c.patient_data).Returns(mock_patients.Object);

            IEnumerable<Transaction> transactions = DataSource.GetTransactions(compare_date, fake_entities.Object);

            Assert.IsNotNull(transactions);
            Assert.IsTrue(transactions is IQueryable);
            Assert.IsTrue(transactions is IQueryable<Transaction>);

            foreach (Transaction transaction in transactions)
            {
                // There is an email
                Assert.IsTrue(!string.IsNullOrEmpty(transaction.PatientEmail));
                // The appoinment time matches
                Assert.IsTrue(transaction.AppointmentTime == compare_date);
                // There is a transaction with this Id
                appointment_data appt = appts.Find(x => x.appointment_id == transaction.AppointmentId);
                Assert.IsNotNull(appt);
                // There is a patient with the appt's Id
                patient_data patient = patients.Find(x => x.patient_id == appt.patient_id);
                Assert.IsNotNull(patient);
                // Check content
                Assert.IsTrue(transaction.AppointmentId == appt.appointment_id);
                Assert.IsTrue(transaction.AppointmentTime == appt.scheduled);
                Assert.IsTrue(appt.status == "completed");
                Assert.IsTrue(transaction.PatientFirstName == patient.first_name);
                Assert.IsTrue(transaction.PatientLastName == patient.last_name);
                Assert.IsTrue(transaction.PatientId == patient.patient_id);
                Assert.IsTrue(transaction.PatientPhone == patient.phone);
            }

            return transactions;
        }

        private static appointment_data Appointment0 = new appointment_data()
        {
            appointment_id = 1,
            patient_id = 1,
            scheduled = new DateTime(2016, 1, 26),
            status = "completed",
            updatedAt = new DateTime(2016, 1, 26),
        };


        private static appointment_data Appointment1 = new appointment_data()
        {
            appointment_id = 2,
            patient_id = 2,
            scheduled = new DateTime(2016, 1, 26),
            status = "canceled",
            updatedAt = new DateTime(2016, 1, 26)
        };


        private static appointment_data Appointment2 = new appointment_data()
        {
            appointment_id = 3,
            patient_id = 1,
            scheduled = new DateTime(2016, 1, 27),
            status = "completed",
            updatedAt = new DateTime(2016, 1, 25)
        };

         private static patient_data Patient0 = new patient_data()
         {
             age = 25,
             email = "abc@xyz.com",
             first_name = "FirstName",
             last_name = "LastName",
             patient_id = 1,
             phone = "510-555-1234"
         };

        private static patient_data Patient1 = new patient_data()
        {
            age = 33,
            email = "",
            first_name = "FirstName2",
            last_name = "LastName2",
            patient_id = 2,
            phone = "510-555-5678",
        };

        /// <summary>
        /// A copy of patient 0, but with no email.
        /// </summary>
        private static patient_data Patient0NoEmail = new patient_data()
        {
            age = 25,
            email = "",
            first_name = "FirstName",
            last_name = "LastName",
            patient_id = 1,
            phone = "510-555-1234"
        };
    }
}