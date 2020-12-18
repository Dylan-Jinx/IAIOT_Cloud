using AutoMapper;
using IAIOT_alpha_0_1_1.Models;
using IAIOT_alpha_0_1_1.Models.DTO;

namespace IAIOT_alpha_0_1_1.Config
{
    /// <summary>
    /// AutoMapper基本映射配置
    /// </summary>
    public class AutoMapperConfig:Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<UserSignDTO, TSysUsers>();
            CreateMap<SysUserInfoDTO, TSysUsers>();
            CreateMap<UserClaimDTO, LoginStatusDTO>();
            CreateMap<AuthorizationDTO, LoginStatusDTO>();
            CreateMap<UserSignDTO, LoginStatusDTO>();
            CreateMap<TProjects, ProjectInfoDTO>();
            CreateMap<ProjectModifyDTO,TProjects>(); 
            CreateMap<ProjectInfoModifyDTO, TProjects>();
            CreateMap<TDevices,DeviceModifyDTO>();
            CreateMap<DeviceModifyDTO, TDevices>();
            CreateMap<SensorModifyDTO, TSensors>();
            CreateMap<TSensors, SensorModifyDTO>();
        }
    }
}
