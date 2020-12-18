using IAIOT_alpha_0_1_1.Services;

namespace IAIOT_alpha_0_1_1.Models.DTO
{
    public class LoginStatusDTO
    {
        public string Telephone { get; set; }
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string scope { get; set; }
        public string sub { get; set; }
        public int auth_time { get; set; }
        public string idp { get; set; }
        public string id { get; set; }
        public string UserId { get; set; }
        public string phone_number { get; set; }
    }
}
