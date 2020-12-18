using IAIOT_alpha_0_1_1.Models.DTO;
using IAIOT_alpha_0_1_1.ResponseControlMsg;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace IAIOT_alpha_0_1_1.Services
{
    public interface ISysUserServices<T>
    {
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="usertel">用户电话</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        Task<bool> Register(UserSignDTO userRegisterDTO);
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="usertel">用户电话</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        Task<JObject> Login(UserSignDTO userSignDTO);
        /// <summary>
        /// 注销
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<T> LogOut(Guid userId);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sysUserInfoDTO"></param>
        /// <returns></returns>
        Task<T> WriteSysUserInfo(SysUserInfoDTO sysUserInfoDTO);
        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<T> FindUserById(Guid userId);
    }
}
