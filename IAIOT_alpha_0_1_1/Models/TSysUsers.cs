using System;
using System.Collections.Generic;

namespace IAIOT_alpha_0_1_1.Models
{
    public partial class TSysUsers
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? LastLogonDate { get; set; }
    }
}
