using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IAIOT_alpha_0_1_1.Models.DTO
{
    public class ProjectModifyDTO
    {
        [Required]
        public string ProjectName { get; set; }
        [Required]
        public string ProjectType { get; set; }
        [Required]
        public string ProjectTag { get; set; }
        [Required]
        public int? IndustryType { get; set; }
        [Required]
        public int ProtocolType { get; set; }
        public string ProjectInfo { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }
        [Required]
        public Guid CtrUserId { get; set; }
    }
}
