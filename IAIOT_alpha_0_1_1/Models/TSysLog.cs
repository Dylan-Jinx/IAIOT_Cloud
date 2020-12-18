using System;
using System.Collections.Generic;

namespace IAIOT_alpha_0_1_1.Models
{
    public partial class TSysLog
    {
        public int LogId { get; set; }
        public string LogMessage { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? LogType { get; set; }
        public Guid? CtrUserId { get; set; }
    }
}
