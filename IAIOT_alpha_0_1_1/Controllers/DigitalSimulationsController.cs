using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IAIOT_alpha_0_1_1.Models;
using IAIOT_alpha_0_1_1.Models.DTO;
using Microsoft.EntityFrameworkCore;
using IAIOT_alpha_0_1_1.ResponseControlMsg;

namespace IAIOT_alpha_0_1_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DigitalSimulationsController : ControllerBase
    {
        private readonly ILogger<DigitalSimulationsController> _logger;
        private readonly IAIOTCloudContext _context;

        public DigitalSimulationsController(
            ILogger<DigitalSimulationsController> logger,
            IAIOTCloudContext context
            )
        {

            _logger = logger;
            _context = context;
        }
        /// <summary>
        /// 获取当前温度
        /// </summary>
        /// <param name="sensorTag">data_temp/data_humi</param>
        /// <param name="deviceId">2</param>
        /// <returns></returns>
        [HttpGet("getDeviceData")]
        public async Task<IActionResult> getCurrentDeviceData(string sensorTag,int deviceId)
        {
            ResponseCtrMsg<TSensorData> response;
            var result =  await _context.TSensorData.Where(a => a.SensorTag == sensorTag && a.DeviceId == deviceId).ToListAsync();
            if (result.Count > 0)
            {
                response = new ResponseCtrMsg<TSensorData>(Enums.CtrResult.Success)
                {
                    ResultObj = result[result.Count-1]
                };
            }
            else
            {
                response = new ResponseCtrMsg<TSensorData>(Enums.CtrResult.Failure)
                {
                    ResultObj = null
                };
            }
            return Ok(response);
        }

        /// <summary>
        /// 获取设备温度记录
        /// </summary>
        /// <param name="sensorTag">data_temp/data_humi</param>
        /// <param name="deviceId">2</param>
        /// <returns></returns>
        [HttpGet("getDeviceDataRecord")]
        public async Task<IActionResult> getDeviceDataRecord(string sensorTag, int deviceId)
        {
            ResponseCtrMsg<List<TSensorData>> response;
            var result = await _context.TSensorData.Where(a => a.SensorTag == sensorTag && a.DeviceId == deviceId).ToListAsync();
            if (result.Count > 0)
            {
                response = new ResponseCtrMsg<List<TSensorData>>(Enums.CtrResult.Success)
                {
                    ResultObj = result
                };
            }
            else
            {
                response = new ResponseCtrMsg<List<TSensorData>>(Enums.CtrResult.Failure)
                {
                    ResultObj = null
                };
            }
            return Ok(response);
        }
    }
}
