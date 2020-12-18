using AutoMapper;
using IAIOT_alpha_0_1_1.Enums;
using IAIOT_alpha_0_1_1.Models;
using IAIOT_alpha_0_1_1.Models.DTO;
using IAIOT_alpha_0_1_1.ResponseControlMsg;
using IAIOT_alpha_0_1_1.Services;
using Final_project_IAIOTCloud.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IAIOT_alpha_0_1_1.Controllers
{
    /// <summary>
    /// 用户接口的实现
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IServerAddressesFeature _serverAddresses;
        private readonly IAuthenticationService _authenticationService;
        private readonly ISysUserServices<TSysUsers> _userServices;
        private readonly IMapper _mapper;

        public AccountController(
            ILogger<AccountController> logger,
            IServerAddressesFeature serverAddresses,
            IAuthenticationService authenticationService,
            ISysUserServices<TSysUsers> userServices,
            IMapper mapper
            )
        {
            _logger = logger;
            _serverAddresses = serverAddresses;
            _authenticationService = authenticationService;
            _userServices = userServices;
            _mapper = mapper;
        }
        /// <summary>
        /// 根据id获取用户所有信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> CatchInfoById(Guid id)
        {
            var data = await _userServices.FindUserById(id);
            return Ok(new ResponseCtrMsg<TSysUsers>(Enums.CtrResult.Success) { ResultObj = data });
        }
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="userRegister"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserSignDTO userSignDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool responseMsg = await _userServices.Register(userSignDTO);
                    if (responseMsg)
                    {
                        return Ok(new ResponseCtrMsg<object>(CtrResult.Success){});
                    }
                    return BadRequest(null);
                }
                catch (Exception ex)
                {
                    this._logger.LogInformation("An exception has occurred,the exception message:" + ex.Message);
                    return BadRequest(new ResponseCtrMsg<object>(CtrResult.Exception)
                    {
                        ErrorObj = new Error()
                        {
                            ErrorMessage = ex.Message
                        }
                    });
                }
            }
            else
            {
                return BadRequest(new ResponseCtrMsg<object>(CtrResult.Failure) { message = "传入的模型为空" });
            }
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login(UserSignDTO userSignDTO)
        {
            if (ModelState.IsValid)
            {
                JObject datasJson = await _userServices.Login(userSignDTO);

                if (datasJson["error"] is null)
                {
                    AuthorizationDTO authorization = JsonHelper.Deserialize<AuthorizationDTO>(datasJson.ToString());
                    UserClaimDTO userClaim = await _authenticationService.AcquireUserClaim(_serverAddresses.Addresses.FirstOrDefault(), authorization.access_token);
                    if (authorization is null || userClaim is null)
                    {
                        return StatusCode(500);
                    }
                    var loginStatus = _mapper.Map<LoginStatusDTO>(userSignDTO);
                    _mapper.Map(userClaim, loginStatus);
                    _mapper.Map<AuthorizationDTO, LoginStatusDTO>(authorization, loginStatus);
                    return Ok(new ResponseCtrMsg<LoginStatusDTO>(CtrResult.Success)
                    {
                        ResultObj= loginStatus
                    });
                }
                else
                {
                    this._logger.LogInformation("User:" + userSignDTO.Telephone + ",Login Control failed");
                    return BadRequest("账号密码错误");
                }
            }
            else
            {
                this._logger.LogInformation("The passed in model is empty");
                return BadRequest(new ResponseCtrMsg<object>(CtrResult.Failure) { message = "传入的模型为空" });
            }
        }
    }
}
