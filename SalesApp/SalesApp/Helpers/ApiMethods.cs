using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SalesApp.Helpers
{
    public static class ApiMethods
    {      
        public static async Task<string> GetData(string nip)
        {
            string address = "https://infoticon-production-backend-functions.cloudticon.com/company?nip="+nip;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "PASSWORD");
            JObject json = new JObject();

            var response = await client.GetAsync(address);
            return await response.Content.ReadAsStringAsync();
        }

    }
}
