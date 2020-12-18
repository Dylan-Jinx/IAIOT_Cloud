using System.ComponentModel.DataAnnotations;

namespace IAIOT_alpha_0_1_1.Models.DTO
{
    public class ProjectInfoModifyDTO
    {
        public int projectId { get; set; }
        [Required]
        public string ProjectName { get; set; }
        [Required]
        public string ProjectType { get; set; }
        [Required]
        public int ProtocolType { get; set; }
        public string ProjectInfo { get; set; }
    }
}
