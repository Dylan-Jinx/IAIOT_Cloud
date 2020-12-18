using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IAIOT_alpha_0_1_1.Models.DTO
{
    public class SensorModifyDTO
    {
        [Required]
        public int DeviceId { get; set; }
        [Required]
        public string SensorName { get; set; }
        [Required]
        public int ProtocolType { get; set; }
        public string SerializeNum { get; set; }
        [Required]
        public string SensorTag { get; set; }
        public string DataType { get; set; }
        public string Unit { get; set; }
        public DateTime? CreateDate { get; set; }
        [Required]
        public Guid CtrUserId { get; set; }
        [Required]
        public int? ProjectId { get; set; }
    }
}
