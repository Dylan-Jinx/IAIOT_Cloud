using System;
using System.Collections.Generic;

namespace IAIOT_alpha_0_1_1.Models
{
    public partial class TActuator
    {
        public int ActuatorId { get; set; }
        public int DeviceId { get; set; }
        public int ProtocolType { get; set; }
        public string ActuatorName { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? PidOrChannelId { get; set; }
        public Guid CtrUserId { get; set; }
    }
}
