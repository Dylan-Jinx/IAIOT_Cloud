using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IAIOT_alpha_0_1_1.Models.DTO
{
    public class AuthorizationDTO
    {
        /// <summary>
        /// 
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int expires_in { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string token_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string scope { get; set; }
    }
}
