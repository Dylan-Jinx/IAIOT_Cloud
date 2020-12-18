using System.ComponentModel.DataAnnotations;

namespace IAIOT_alpha_0_1_1.Models.DTO
{
    public class UserSignDTO
    {
        [Required]
        public string Telephone { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
