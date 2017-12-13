using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace NMCT.Cloud1.GarbageApplication
{
    public static class BezoekersService
    {
        private static string CONNECTIONSTRING = Environment.GetEnvironmentVariable("ConnectionString");

        [FunctionName("GetDagen")]
        public static HttpResponseMessage GetDagen([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "days/")]HttpRequestMessage req, TraceWriter log)
        {
            try
            {
                List<string> days = new List<string>();
                using (SqlConnection connenction = new SqlConnection(CONNECTIONSTRING))
                {
                    connenction.Open();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connenction;
                        string sql = "SELECT DISTINCT DagVanDeWeek FROM Bezoekers";
                        command.CommandText = sql;

                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                            days.Add(reader["DagVanDeWeek"].ToString());
                    }
                    return req.CreateResponse(HttpStatusCode.OK, days);
                }
            }
            catch(Exception ex)
            {
                return req.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
