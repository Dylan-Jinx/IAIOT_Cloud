using AutoMapper;
using IAIOT_alpha_0_1_1.Models;
using IAIOT_alpha_0_1_1.Models.DTO;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IAIOT_alpha_0_1_1.Services
{
    public class ProjectsService : IProjectsService<TProjects>
    {
        private readonly ILogger<ProjectsService> _logger;
        private readonly IServerAddressesFeature _serverAddresses;
        private readonly IAuthenticationService _authenticationService;
        private readonly IAIOTCloudContext _context;
        private readonly IMapper _mapper;
        public ProjectsService
            (
            IAIOTCloudContext context,
            IServerAddressesFeature serverAddresses,
            IAuthenticationService authenticationService,
            ILogger<ProjectsService> logger,
            IMapper mapper
            )
        {
            _logger = logger;
            _context = context;
            _serverAddresses = serverAddresses;
            _authenticationService = authenticationService;
            _mapper = mapper;
        }
        public async Task<List<TProjects>> FindAllProjectByUserId(Guid userId)
        {
            return await _context.TProjects.Where(a => a.CtrUserId == userId).ToListAsync();
        }

        public async Task<TProjects> FindProjectById(int projectId)
        {
            return await _context.TProjects.FindAsync(projectId);
        }

        public async Task<bool> InsertProject(ProjectModifyDTO projectModify)
        {
            TProjects projects = new TProjects();
            _mapper.Map(projectModify, projects);
            await _context.TProjects.AddAsync(projects);
            bool flag = await _context.SaveChangesAsync() > 0;
            return flag;
        }

        public async Task<bool> RemoveProject(int projectId)
        {
            bool flag = false;
            TProjects project = await FindProjectById(projectId);
            if (project != null)
            {
                _context.TProjects.Remove(project);
                flag =await _context.SaveChangesAsync()>0;
            }
            return flag;
        }

        public async Task<bool> UpdateProject(ProjectInfoModifyDTO projectInfoModifyDTO)
        {
            bool flag = false;
            TProjects projects = await FindProjectById(projectInfoModifyDTO.projectId);
            projects.ProjectName = projectInfoModifyDTO.ProjectName;
            projects.ProtocolType = projectInfoModifyDTO.ProtocolType;
            projects.ProjectInfo = projectInfoModifyDTO.ProjectInfo;
            projects.ProjectType = projectInfoModifyDTO.ProjectType;
            _context.Update(projects);
            flag = await _context.SaveChangesAsync() > 0;
            return flag;
        }
    }
}
