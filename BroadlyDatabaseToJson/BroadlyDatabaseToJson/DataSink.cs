using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

using System.Web.Helpers;

namespace BroadlyDatabaseToJson
{
    /// <summary>
    /// 
    /// </summary>
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

        public void Sink(object PublishingItem)
        {
            try
            {
                //string coded_item = System.Web.Helpers.Json.Encode(PublishingItem);
                string coded_item = Newtonsoft.Json.JsonConvert.SerializeObject(PublishingItem);
                SendHttpRequest(coded_item);

                Console.WriteLine(coded_item);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void SendHttpRequest(string content)
        {
            string result = MakeRequest(content);
            // Console.WriteLine(result);
        }

        private static string MakeRequest(string content)
        {
            // pulled some from: 
            // http://stackoverflow.com/questions/5527316/how-to-set-the-content-of-an-httpwebrequest-in-c
            UriBuilder uri = new UriBuilder("http://requestb.in/suik15su");

            HttpContent stringContent = new StringContent(content);
            using (var client = new HttpClient())
            {
                var response = client.PostAsync(uri.Uri, stringContent).Result;
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                return response.Content.ReadAsStringAsync().Result;
            }
        }
    }    
}
