using System;
using System.Collections.Generic;


namespace BroadlyDatabaseToJson
{
    class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // TODO: Adjust DateTime for business case.  Make configurable to look for previous dates.
            // TODO: Also, look at pulling out 
            IEnumerable<Transaction> transactions = DataSource.Instance.GetTransactions(new DateTime(2016, 1, 29));
            // TODO: Move the Uri to App.Settings or command line
            DataSink.Sink(transactions, new Uri("http://requestb.in/suik15su"));

            //Console.ReadKey();
        }
    }
}
