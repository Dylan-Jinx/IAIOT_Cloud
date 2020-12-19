using System;
using System.Collections.Generic;

namespace IAIOT_alpha_0_1_1.Models
{
    public partial class TSensorData
    {
        public int SensorDataId { get; set; }
        public int? DeviceId { get; set; }
        public string DeviceName { get; set; }
        public int? SensorId { get; set; }
        public string SensorName { get; set; }
        public string SensorUnit { get; set; }
        public string SensorData { get; set; }
        public int? DataType { get; set; }
        public string SensorTag { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? ProjectId { get; set; }
    }
}
