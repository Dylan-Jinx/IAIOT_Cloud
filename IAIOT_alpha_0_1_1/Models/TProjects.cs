using System;
using System.Collections.Generic;

namespace IAIOT_alpha_0_1_1.Models
{
    public partial class TProjects
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectTag { get; set; }
        public string ProjectType { get; set; }
        public int? IndustryType { get; set; }
        public int ProtocolType { get; set; }
        public string ProjectInfo { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid CtrUserId { get; set; }
    }
}
