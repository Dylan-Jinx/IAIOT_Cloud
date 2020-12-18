using IAIOT_alpha_0_1_1.Models.DTO;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace IAIOT_alpha_0_1_1.Services
{
    public interface IAuthenticationService
    {
        Task<JObject> AcquireAccessToken(string identityServerIp, string usertel, string password);
        Task<UserClaimDTO> AcquireUserClaim(string identityServerIp, string token);
    }
}
