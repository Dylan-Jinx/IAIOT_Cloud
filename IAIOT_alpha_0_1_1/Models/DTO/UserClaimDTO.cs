namespace IAIOT_alpha_0_1_1.Models.DTO
{
    public class UserClaimDTO
    {

        public string sub { get; set; }
        public int auth_time { get; set; }
        public string idp { get; set; }
        public string id { get; set; }
        public string UserId { get; set; }
        public string phone_number { get; set; }
        public string amr { get; set; }
    }
}