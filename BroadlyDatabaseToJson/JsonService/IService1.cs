using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using BroadlyDatabaseToJson;

namespace WcfJsonRestService
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        Transaction GetData(string id);
    }

    public class Service1 : IService1
    {
        [WebInvoke(Method = "GET",
                    ResponseFormat = WebMessageFormat.Json,
                    UriTemplate = "data/{id}")]
        public Transaction GetData(string id)
        {
            // lookup person with the requested id 
            return new Transaction()
            {
                AppointmentId = 1,
                PatientId = 1,
                PatientFirstName = "FirstName",
                AppointmentTime = DateTime.Now,
                PatientEmail = "abc@xyz.com",
                PatientLastName = "LastName",
                PatientPhone = "Phone"
            };
        }
    }
}

namespace JsonService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: Add your service operations here
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
