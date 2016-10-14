using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CalcService
{
    class Program
    {

        static void Main(string[] args)
        {
            var serviceHost = new ServiceHost(typeof(Calculator));
            serviceHost.Open();

            Console.WriteLine("Service started");
            Console.WriteLine("Press any key to close");
            Console.Read();

            serviceHost.Close();
        }
    }
}
