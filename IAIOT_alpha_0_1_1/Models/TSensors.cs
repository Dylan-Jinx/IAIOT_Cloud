using System;
using System.Collections.Generic;

namespace IAIOT_alpha_0_1_1.Models
{
    public partial class TSensors
    {
        public int SensorId { get; set; }
        public int DeviceId { get; set; }
        public string SensorName { get; set; }
        public int ProtocolType { get; set; }
        public string SerializeNum { get; set; }
        public string SensorTag { get; set; }
        public string DataType { get; set; }
        public string Unit { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid CtrUserId { get; set; }
        public int? ProjectId { get; set; }
    }
}
