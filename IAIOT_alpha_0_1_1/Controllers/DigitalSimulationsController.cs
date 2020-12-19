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
            var result = await _context.TSensorData.Where(a => a.SensorTag == sensorTag && a.DeviceId == deviceId)
                .OrderByDescending(a => a.CreateDate)
                .FirstOrDefaultAsync();
            if (result!=null)
            {
                response = new ResponseCtrMsg<TSensorData>(Enums.CtrResult.Success)
                {
                    ResultObj = result
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
        public async Task<IActionResult> getDeviceDataRecord(string sensorTag, int deviceId,int count=10)
        {
            ResponseCtrMsg<List<TSensorData>> response;
            var result = await _context.TSensorData.Where(a => a.SensorTag == sensorTag && a.DeviceId == deviceId)
                .OrderByDescending(a=>a.CreateDate)
                .Take(count).ToListAsync();
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
            return await Task.FromResult<IActionResult>(Ok(response));
        }


        /// <summary>
        /// 获取设备上报数据数量
        /// </summary>
        /// <param name="projectId">1</param>
        /// <returns></returns>
        [HttpGet("getSensorDataCountByProjectId")]
        public async Task<IActionResult> getSensorDataCountByProjectId(int projectId)
        {
            ResponseCtrMsg<int> response;
            var result = await _context.TSensorData.Where(a => a.ProjectId == projectId).CountAsync();
            if (result > 0)
            {
                response = new ResponseCtrMsg<int>(Enums.CtrResult.Success)
                {
                    ResultObj = result
                };
            }
            else
            {
                response = new ResponseCtrMsg<int>(Enums.CtrResult.Failure)
                {
                    ResultObj = 0
                };
            }
            return await Task.FromResult<IActionResult>(Ok(response));
        }

        /// <summary>
        /// 获取设备上报数据数量
        /// </summary>
        /// <param name="projectId">1</param>
        /// <returns></returns>
        [HttpGet("getDeviceCountByProjectId")]
        public async Task<IActionResult> getDeviceCountByProjectId(int projectId)
        {
            ResponseCtrMsg<int> response;
            var result = await _context.TDevices.Where(a => a.ProjectId == projectId).CountAsync();
            if (result > 0)
            {
                response = new ResponseCtrMsg<int>(Enums.CtrResult.Success)
                {
                    ResultObj = result
                };
            }
            else
            {
                response = new ResponseCtrMsg<int>(Enums.CtrResult.Failure)
                {
                    ResultObj = 0
                };
            }
            return await Task.FromResult<IActionResult>(Ok(response));
        }

        /// <summary>
        /// 获取设备上报数据数量
        /// </summary>
        /// <param name="projectId">1</param>
        /// <returns></returns>
        [HttpPost("getSensorCountByProjectId")]
        public async Task<IActionResult> getSensorCountByProjectId(List<int> projectId)
        {
            List<int> userProjectIdDeviceCount = new List<int>(); 
            ResponseCtrMsg<List<int>> response;
            foreach(var temp in projectId)
            {
                var result = await _context.TSensors.Where(a => a.ProjectId == temp).CountAsync();
                userProjectIdDeviceCount.Add(result);
            }
            if (userProjectIdDeviceCount.Count > 0)
            {
                response = new ResponseCtrMsg<List<int>>(Enums.CtrResult.Success)
                {
                    ResultObj = userProjectIdDeviceCount
                };
            }
            else
            {
                response = new ResponseCtrMsg<List<int>>(Enums.CtrResult.Failure)
                {
                    ResultObj = null
                };
            }
            return await Task.FromResult<IActionResult>(Ok(response));
        }
    }
}
