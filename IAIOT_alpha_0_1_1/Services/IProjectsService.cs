using IAIOT_alpha_0_1_1.Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IAIOT_alpha_0_1_1.Services
{
    public interface IProjectsService<T>
    {
        /// <summary>
        /// 用项目ID查询项目信息
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        Task<T> FindProjectById(int projectId);
        /// <summary>
        /// 用用户ID查询创建的所有项目
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<T>> FindAllProjectByUserId(Guid userId);
        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="projectInfoDTO"></param>
        /// <returns></returns>
        Task<bool> InsertProject(ProjectModifyDTO projectInfoDTO);
        /// <summary>
        /// 移除项目
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        Task<bool> RemoveProject(int projectId);
        /// <summary>
        /// 更新项目
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        Task<bool> UpdateProject(ProjectInfoModifyDTO projectInfoModifyDTO);
    }
}
