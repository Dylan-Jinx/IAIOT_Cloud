using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IAIOT_alpha_0_1_1.Models.DTO
{
    public class DeviceModifyDTO
    {
        [Required]
        public string DeviceName { get; set; }
        [Required]
        public int ProtocolType { get; set; }
        [Required]
        public string DeviceTag { get; set; }
        [Required]
        public int? IsPublic { get; set; }
        [Required]
        public int? DataReportState { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? CtrUserId { get; set; }
        [Required]
        public int ProjectId { get; set; }
    }
}
