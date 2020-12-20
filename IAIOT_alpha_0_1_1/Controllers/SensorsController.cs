using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IAIOT_alpha_0_1_1.Enums;
using IAIOT_alpha_0_1_1.Models;
using IAIOT_alpha_0_1_1.Models.DTO;
using IAIOT_alpha_0_1_1.ResponseControlMsg;
using IAIOT_alpha_0_1_1.Services;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IAIOT_alpha_0_1_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorsController : ControllerBase
    {
        private readonly ILogger<SensorsController> _logger;
        private readonly IServerAddressesFeature _serverAddresses;
        private readonly ISensorService<TSensors> _sensorService;
        private readonly IMapper _mapper;

        public SensorsController(
            ILogger<SensorsController> logger,
            IServerAddressesFeature serverAddresses,
            ISensorService<TSensors> sensorService,
            IMapper mapper
            )
        {
            _logger = logger;
            _serverAddresses = serverAddresses;
            _sensorService = sensorService;
            _mapper = mapper;
        }
        [HttpGet("{sensorId}")]
        public async Task<IActionResult> FindSensorBySensorId(int sensorId)
        {
            var sensor = await _sensorService.FindSensorBySensorId(sensorId);
            SensorModifyDTO sensorModifyDTO = new SensorModifyDTO();
            _mapper.Map(sensor, sensorModifyDTO);
            return Ok(new ResponseCtrMsg<SensorModifyDTO>(CtrResult.Success) { ResultObj = sensorModifyDTO });
        }
        [HttpGet("ProjectId")]
        public async Task<IActionResult> FindSensorByProjectId(int projectId)
        {
            var sensors = await _sensorService.FindAllSensorsByProjectId(projectId);
            List<SensorModifyDTO> sensorModifyDTO = new List<SensorModifyDTO>();
            _mapper.Map(sensors, sensorModifyDTO);
            return Ok(new ResponseCtrMsg<List<SensorModifyDTO>>(CtrResult.Success) { ResultObj = sensorModifyDTO });
        }
        [HttpGet("UserId")]
        public async Task<IActionResult> FindSensorBySensorId(Guid userId)
        {
            var sensors = await _sensorService.FindAllSensorByUserId(userId); 
            List<SensorModifyDTO> sensorModifyDTO = new List<SensorModifyDTO>();
            _mapper.Map(sensors, sensorModifyDTO);
            return Ok(new ResponseCtrMsg<List<SensorModifyDTO>>(CtrResult.Success) { ResultObj = sensorModifyDTO });
        }

        [HttpPost]
        public async Task<IActionResult> addSensor(SensorModifyDTO sensorModifyDTO)
        {
            if (ModelState.IsValid)
            {
                bool flag = await _sensorService.InsertSensor(sensorModifyDTO);
                if (flag)
                {
                    return Ok(new ResponseCtrMsg<TSensors>(CtrResult.Success) { });
                }
            }
            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> updateSensor(TSensors sensor)
        {
            if (ModelState.IsValid)
            {
                bool flag = await _sensorService.UpdateSensor(sensor);
                if (flag)
                {
                    return Ok(new ResponseCtrMsg<TSensors>(CtrResult.Success) { });
                }
            }
            return BadRequest();
        }
        [HttpDelete]
        public async Task<IActionResult> removeSensor(int sensorId)
        {
            if (ModelState.IsValid)
            {
                bool flag = await _sensorService.RemoveSensor(sensorId);
                if (flag)
                {
                    return Ok(new ResponseCtrMsg<TSensors>(CtrResult.Success) { });
                }
            }
            return BadRequest();
        }


        [HttpGet("DeviceId")]
        public async Task<IActionResult> getSensorByProjectId(int deviceId)
        {
            if (ModelState.IsValid)
            {
                List<TSensors> sensors= await _sensorService.FindAllSensorsByDevicetId(deviceId);
                if (sensors.Count> 0 )
                {
                    return Ok(new ResponseCtrMsg<List<TSensors>>(CtrResult.Success) { ResultObj = sensors });
                }
                else
                {
                    return Ok(new ResponseCtrMsg<List<TSensors>>(CtrResult.Failure) { ResultObj = null });
                }
            }
            return BadRequest();
        }
    }
}
