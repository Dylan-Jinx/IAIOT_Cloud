using System;
using System.Collections.Generic;

namespace IAIOT_alpha_0_1_1.Models
{
    public partial class TDevices
    {
        public int DeviceId { get; set; }
        public string DeviceName { get; set; }
        public int ProtocolType { get; set; }
        public string DeviceTag { get; set; }
        public int? IsPublic { get; set; }
        public int? DataReportState { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid CtrUserId { get; set; }
        public int ProjectId { get; set; }
    }
}
