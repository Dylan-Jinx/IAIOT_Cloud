using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IAIOT_alpha_0_1_1.Config
{
    public class IdentityServerConfig
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        public static IEnumerable<ApiScope> GetApiResources()
        {
            return new List<ApiScope>
           {
               new ApiScope("api1", "My API")
           };
        }
        /// <summary>
        /// 哪些客户端 Client（应用） 可以使用这个 Authorization Server
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
               new Client
               {
                   ClientId = "client1",
                   AllowedGrantTypes = GrantTypes.ClientCredentials,

                   ClientSecrets =
                   {
                       new Secret("secret".Sha256())
                   },
                   AllowedScopes =
                   {
                       IdentityServerConstants.StandardScopes.OpenId, //必须要添加，否则报forbidden错误
                       IdentityServerConstants.StandardScopes.Profile
                   },
               },
 
               // resource owner password grant client
               new Client
               {
                   ClientId = "client2",
                   AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                   ClientSecrets =
                   {
                       new Secret("secret".Sha256())
                   },
                   AllowedScopes =
                   {
                       IdentityServerConstants.StandardScopes.OpenId, //必须要添加，否则报forbidden错误
                       IdentityServerConstants.StandardScopes.Profile
                   }
               }
            };
        }
    }
}
