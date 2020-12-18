using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IAIOT_alpha_0_1_1.Controllers
{
    /// <summary>
    /// 心跳  设备健康检查接口的实现
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        /// <summary>
        /// 心跳检测:用于确认设备或者是客户端是否与服务器连接
        /// TODO:Heartbeat detection-Used to confirm whether the device or client is connected to the server
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Check()
        {
            return Ok();
        }
    }
}
