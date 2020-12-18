using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IAIOT_alpha_0_1_1.Models.DTO
{
    public class ProjectInfoDTO
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectType { get; set; }
        public int? IndustryType { get; set; }
        public int ProtocolType { get; set; }
        public string ProjectInfo { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
