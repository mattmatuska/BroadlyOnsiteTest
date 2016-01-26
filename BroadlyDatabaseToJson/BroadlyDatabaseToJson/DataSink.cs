using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web.Helpers;

namespace BroadlyDatabaseToJson
{
    public sealed class DataSink
    {
        private static volatile DataSink instance;
        private static object syncRoot = new Object();

        private DataSink() { }

        public static DataSink Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new DataSink();
                    }
                }

                return instance;
            }
        }

        public void Send(object PublishingItem)
        {
            try
            {
                string coded_item = Newtonsoft.Json.JsonConvert.SerializeObject(PublishingItem);

                Console.WriteLine(coded_item);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }    
}
