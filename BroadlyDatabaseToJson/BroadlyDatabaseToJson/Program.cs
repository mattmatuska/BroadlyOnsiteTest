using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BroadlyDatabaseToJson
{
    class Program
    {
        static void Main(string[] args)
        {
            DataSource source = DataSource.Instance; 
            var orders = source.GetDataOrders(DateTime.Now);
            DataSink sink = DataSink.Instance;
            sink.Send(orders);

            Console.ReadKey();
        }
    }
}
