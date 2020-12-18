using AutoMapper;
using IAIOT_alpha_0_1_1.Models;
using IAIOT_alpha_0_1_1.Models.DTO;
using IAIOT_alpha_0_1_1.ResponseControlMsg;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IAIOT_alpha_0_1_1.Services
{
    public class SysUserServices : ISysUserServices<TSysUsers>
    {
        private readonly ILogger<SysUserServices> _logger;
        private readonly IServerAddressesFeature _serverAddresses;
        private readonly IAuthenticationService _authenticationService;
        private readonly IAIOTCloudContext _context;
        private readonly IMapper _mapper;
        public SysUserServices
            (
            IAIOTCloudContext context,
            IServerAddressesFeature serverAddresses,
            IAuthenticationService authenticationService,
            ILogger<SysUserServices> logger,
            IMapper mapper
            )
        {
            _logger = logger;
            _context = context;
            _serverAddresses = serverAddresses;
            _authenticationService = authenticationService;
            _mapper = mapper;
        }
        public async Task<TSysUsers> FindUserById(Guid userId)
        {
            return await _context.TSysUsers.FindAsync(userId);
        }

        public async Task<JObject> Login(UserSignDTO userSignDTO)
        {
            JObject responseJson = null;
            try
            {
                responseJson = await _authenticationService.AcquireAccessToken
                    (_serverAddresses.Addresses.FirstOrDefault(), userSignDTO.Telephone, userSignDTO.Password);
            }
            catch (Exception ex)
            {
                this._logger.LogError("An exception occured,the exception message:" + ex.Message);
            }
            return responseJson;
        }

        public Task<TSysUsers> LogOut(Guid userId)
        {
            throw new NotImplementedException();
        }
        
        public async Task<bool> Register(UserSignDTO userSignDTO)
        {
            TSysUsers users=new TSysUsers();
            _mapper.Map(userSignDTO, users);
            users.UserId = Guid.NewGuid();
            //TSysUsers sysUsers = new TSysUsers()
            //{
            //    UserId = Guid.NewGuid(),
            //    CreateDate = DateTime.Now,
            //    Telephone = userSignDTO.Telephone,
            //    Password = userSignDTO.Password
            //};
            await _context.TSysUsers.AddAsync(users);
            bool flag =  _context.SaveChanges() > 0;
            return flag;
        }

        public Task<TSysUsers> WriteSysUserInfo(SysUserInfoDTO sysUserInfoDTO)
        {
            throw new NotImplementedException();
        }
    }
}
