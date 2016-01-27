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
    /// Sink for the data-- send it out of the system into wherever it's going. 
    /// In the future, depending on usage, this may become a Singleton instead of a static class. 
    /// But, at the moment the publishing is straightforward.
    /// </summary>
    public static class DataSink
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        static DataSink() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ContentObject">The object to publish.</param>
        /// <param name="uri">An uri at which to post the content.</param>
        public static void Sink(object ContentObject, Uri uri)
        {
            try
            {
                // didn't use the built-in encoder because it doesn't convert the date properly. 
                //string coded_item = System.Web.Helpers.Json.Encode(PublishingItem);
                string content_as_json = Newtonsoft.Json.JsonConvert.SerializeObject(ContentObject);
                SendHttpRequest(content_as_json, uri);

                Console.WriteLine(content_as_json);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Posts data via HTTP request.
        /// </summary>
        /// <param name="content">Content to post.</param>
        /// <param name="uri">An uri at which to post the content.</param>
        /// <returns></returns>
        private static string SendHttpRequest(string content, Uri uri)
        {
            // Used an answer from here for the basis of this: 
            // http://stackoverflow.com/questions/5527316/how-to-set-the-content-of-an-httpwebrequest-in-c

            HttpContent stringContent = new StringContent(content);
            using (var client = new HttpClient())
            {
                var response = client.PostAsync(uri, stringContent).Result;
                if (!response.IsSuccessStatusCode)
                {
                    return "";
                }

                return response.Content.ReadAsStringAsync().Result;
            }
        }
    }    
}
