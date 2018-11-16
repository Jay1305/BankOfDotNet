using IdentityModel.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BankOfDotNet.Console
{
    class Program
    {
        static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        private static async Task MainAsync()
        {
            // discover all endpoints using metadata of identity server

            var disco = await DiscoveryClient.GetAsync("http://localhost:8000");

            if (disco.IsError)
            {
                System.Console.WriteLine(disco.Error);
                return;
            }

            // grab a bearer token
            var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");

            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("bankOfDotNetApi");

            if (tokenResponse.IsError)
            {
                System.Console.WriteLine(tokenResponse.Error);
                return;
            }

            System.Console.WriteLine(tokenResponse.Json);

            System.Console.WriteLine("\n\n");

            // consume our customer API
            using (var client = new HttpClient())
            {
                client.SetBearerToken(tokenResponse.AccessToken);

                var customerInfo = new StringContent(
                    JsonConvert.SerializeObject( new { Id = 2, FirstName="Jay1", LastName ="Pandit1" }),System.Text.Encoding.UTF8,"application/json");

                var createCustomerResponse = await client.PostAsync("http://localhost:5000/api/customers",
                                                                    customerInfo);

                if (!createCustomerResponse.IsSuccessStatusCode)
                {
                    System.Console.WriteLine(createCustomerResponse.StatusCode);
                }

                var getCustomerResponse = await client.GetAsync("http://localhost:5000/api/customers");

                if (!getCustomerResponse.IsSuccessStatusCode)
                {
                    System.Console.WriteLine(getCustomerResponse.StatusCode);
                }
                else
                {
                    var content = await getCustomerResponse.Content.ReadAsStringAsync();
                    System.Console.WriteLine(JArray.Parse(content));
                }

                System.Console.ReadLine();
            }


        }
    }
}
