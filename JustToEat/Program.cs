using System;
using JustToEat.CallApi;
using System.IO;
using Microsoft.Extensions.Configuration;


namespace JustToEat
{
    class Program
    {
        public static IConfiguration Configuration { get; set; }

        static void Main(string[] args)
        {
            Output output = new SystemOutput();
            output.Write("Please enter postcode: ");
            string query = Console.ReadLine();

            LoadConfiguration();

            try {
                CallRestarauntAPI RestarauntAPI = new CallRestarauntAPI();
                RestarauntAPI.Get(query);
            } catch(Exception e) {
                string error = "Oops, Something went wrong. Error is :\n";
                output.Write(error + e.Message);
                // Log error here
            }

            Console.Read();
        }

        private static void LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsetting.json");

            Configuration = builder.Build();
        }
    }
}
