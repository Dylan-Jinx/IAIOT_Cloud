using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IAIOT_alpha_0_1_1.Models.DTO
{
    public class SenSorDataDTO
    {

        public int? DeviceId { get; set; }
        public string DeviceName { get; set; }
        public int? SensorId { get; set; }
        public string SensorName { get; set; }
        public string SensorUnit { get; set; }
        public string SensorData { get; set; }
        public int? DataType { get; set; }
        public string SensorTag { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
