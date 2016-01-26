using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BroadlyDatabaseToJson
{
    /// <summary>
    /// POCO Transaction.  
    /// </summary>
    public class Transaction
    {
        public Transaction() { }

        public Transaction( int AppointmentId, 
                            DateTime? AppointmentTime,
                            int PatientId,
                            string PatientFirstName, 
                            string PatientLastName,
                            string PatientEmail,
                            string PatientPhone = "")
        {
            this.AppointmentId = AppointmentId;
            this.AppointmentTime = AppointmentTime;
            this.PatientId = PatientId;
            this.PatientFirstName = PatientFirstName;
            this.PatientLastName = PatientLastName;
            this.PatientEmail = PatientEmail;
            this.PatientPhone = PatientPhone;
        }

        public int AppointmentId { get; set; }
        public DateTime? AppointmentTime { get; set; }
        public int PatientId { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public string PatientEmail { get; set; }
        public string PatientPhone { get; set; }
    }
}
