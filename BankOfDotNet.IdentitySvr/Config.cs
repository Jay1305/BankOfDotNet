using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityServer4.Models;

namespace BankOfDotNet.IdentitySvr
{
    public class Config
    {

        public static IEnumerable<ApiResource> GetAllApiResources()
        {
            return new List<ApiResource> { new ApiResource("bankOfDotNetApi","Customer API for BankOfDotNet")};
        }

        public static IEnumerable<Client> GetClient()
        {
            return new List<Client> {
                new Client {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = {new Secret("secret".Sha256()) },
                    AllowedScopes={"bankOfDotNetApi" }
                } };
        }
    }
}
