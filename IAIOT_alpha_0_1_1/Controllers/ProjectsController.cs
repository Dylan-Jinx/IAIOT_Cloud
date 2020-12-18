using AutoMapper;
using IAIOT_alpha_0_1_1.Enums;
using IAIOT_alpha_0_1_1.Models;
using IAIOT_alpha_0_1_1.Models.DTO;
using IAIOT_alpha_0_1_1.ResponseControlMsg;
using IAIOT_alpha_0_1_1.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IAIOT_alpha_0_1_1.Controllers
{
    /// <summary>
    /// 项目接口的实现
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly ILogger<ProjectsController> _logger;
        private readonly IServerAddressesFeature _serverAddresses;
        private readonly IProjectsService<TProjects> _projectsService;
        private readonly IMapper _mapper;

        public ProjectsController(
            ILogger<ProjectsController> logger,
            IServerAddressesFeature serverAddresses,
            IProjectsService<TProjects> projectsService,
            IMapper mapper
            )
        {
            _logger = logger;
            _serverAddresses = serverAddresses;
            _projectsService = projectsService;
            _mapper = mapper;
        }
        [HttpGet("{projectId}")]
        public async Task<IActionResult> findProjectById(int projectId)
        {
            var project = await _projectsService.FindProjectById(projectId);
            ProjectInfoDTO projectInfoDTO = new ProjectInfoDTO();
            _mapper.Map(project, projectInfoDTO);
            return Ok(new ResponseCtrMsg<ProjectInfoDTO>(CtrResult.Success) { ResultObj = projectInfoDTO });
        }
        [HttpGet("byuserid")]
        public async Task<IActionResult> findProjectsByUserId(Guid userId)
        {
            var project = await _projectsService.FindAllProjectByUserId(userId);
            List<ProjectInfoDTO> projectInfoDTO = new List<ProjectInfoDTO>();
            _mapper.Map(project, projectInfoDTO);
            return Ok(new ResponseCtrMsg<List<ProjectInfoDTO>>(CtrResult.Success) { ResultObj = projectInfoDTO });
        }
        [HttpPost]
        public async Task<IActionResult> addProject(ProjectModifyDTO project)
        {
            if (ModelState.IsValid)
            {
                bool flag = await _projectsService.InsertProject(project);
                if (flag)
                {
                    return Ok(new ResponseCtrMsg<List<ProjectInfoDTO>>(CtrResult.Success) { });
                }
                else
                {
                    return BadRequest(new ResponseCtrMsg<List<ProjectInfoDTO>>(CtrResult.Failure) { });
                }
            }
            else
            {
                return BadRequest(new ResponseCtrMsg<List<ProjectInfoDTO>>(CtrResult.Failure) { });
            }
        }
        [HttpDelete]
        public async Task<IActionResult> removeProject(int projectId)
        {
            if (ModelState.IsValid)
            {
                bool flag = await _projectsService.RemoveProject(projectId);
                if (flag)
                {
                    return Ok(new ResponseCtrMsg<List<ProjectInfoDTO>>(CtrResult.Success) { });
                }
                else
                {
                    return BadRequest(new ResponseCtrMsg<List<ProjectInfoDTO>>(CtrResult.Failure) { });
                }
            }
            else
            {
                return BadRequest(new ResponseCtrMsg<List<ProjectInfoDTO>>(CtrResult.Failure) { });
            }
        }
        [HttpPut]
        public async Task<IActionResult> updateProject(ProjectInfoModifyDTO projectInfo)
        {
            bool flag = await _projectsService.UpdateProject(projectInfo);
            if (flag)
            {
                return Ok(new ResponseCtrMsg<List<ProjectInfoDTO>>(CtrResult.Success) { });
            }
            else
            {
                return BadRequest(new ResponseCtrMsg<List<ProjectInfoDTO>>(CtrResult.Failure) { });
            }
        }
    }
}
