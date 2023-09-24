using Microsoft.Azure.OperationalInsights;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LogAnalytic
{
    public class Program
    {
        static void Main(string[] args)
        {
            String ApiVersion = "2016-04-01";
            string json = "My logs";
            var workspaceId = "605ef956-aa2c-4d15-879f-cfc083ba3b47";
            var workspaceKey = "nsaQdLbjDkJfXrYW405u2S8YjWie5E3hC805ZAdoX+qUxvpUcb0DDDq4GncbjoCsTt9n6fkHI7K/B+n8B4PJ7Q==";
            OperationalInsightsDataClient client = new OperationalInsightsDataClient(workspaceId, workspaceKey);
        }

        private static string GetSignature(string method, int contentLength, string contentType, string date, string resource,string SharedKey,string WorkspaceId)
        {
            string message = $"{method}\n{contentLength}\n{contentType}\nx-ms-date:{date}\n{resource}";
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            using (HMACSHA256 encryptor = new HMACSHA256(Convert.FromBase64String(SharedKey)))
            {
                return $"SharedKey {WorkspaceId}:{Convert.ToBase64String(encryptor.ComputeHash(bytes))}";
            }
        }
    }
}
