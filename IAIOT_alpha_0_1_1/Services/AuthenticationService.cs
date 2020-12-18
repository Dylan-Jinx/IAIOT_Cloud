using Final_project_IAIOTCloud.Utility;
using IAIOT_alpha_0_1_1.Models.DTO;
using IdentityModel.Client;
using IdentityServer4.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace IAIOT_alpha_0_1_1.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ILogger<AuthenticationService> _logger;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="accountService"></param>
        public AuthenticationService(
            ILogger<AuthenticationService> logger
            )
        {
            _logger = logger;
        }
        public async Task<JObject> AcquireAccessToken(string identityServerIp, string usertel, string password)
        {
            var client = new HttpClient();
            var discoveryClient = new DiscoveryDocumentRequest()
            {
                Address = identityServerIp,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            };
            var disco = await client.GetDiscoveryDocumentAsync(discoveryClient);
            if (disco.IsError)
            {
                this._logger.LogError(disco.Error);
            }
            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "client2",
                ClientSecret = "secret",
                GrantType = GrantType.ResourceOwnerPassword,
                UserName = usertel,
                Password = password
            });
            return tokenResponse.Json;
        }

        public async Task<UserClaimDTO> AcquireUserClaim(string identityServerIp, string token)
        {

            HttpRequestEntity httpRequestEntity = new HttpRequestEntity()
            {
                Method = Enums.HttpMethod.GET
            };
            HttpResponseEntity responseEntity = HttpHelper.Get(identityServerIp + "/connect/userinfo", httpRequestEntity, token);
            return await Task.FromResult(JsonHelper.Deserialize<UserClaimDTO>(responseEntity.Bodys.ToString()));
        }
    }
}
