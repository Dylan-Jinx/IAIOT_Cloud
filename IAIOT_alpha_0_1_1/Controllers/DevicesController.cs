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
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class DevicesController : ControllerBase
    {
        private readonly ILogger<DevicesController> _logger;
        private readonly IServerAddressesFeature _serverAddresses;
        private readonly IDevicesService<TDevices> _devicesService;
        private readonly IMapper _mapper;

        public DevicesController(
            ILogger<DevicesController> logger,
            IServerAddressesFeature serverAddresses,
            IDevicesService<TDevices> devicesService,
            IMapper mapper
            )
        {
            _logger = logger;
            _serverAddresses = serverAddresses;
            _devicesService = devicesService;
            _mapper = mapper;
        }

        [HttpGet("{deviceId}")]
        public async Task<IActionResult> getDevicesForDeviceId(int deviceId)
        {
            var device = await  _devicesService.FindDeviceByDeviceId(deviceId);
            DeviceModifyDTO deviceModifyDTO = new DeviceModifyDTO();
            _mapper.Map(device,deviceModifyDTO);
            return Ok(new ResponseCtrMsg<DeviceModifyDTO>(CtrResult.Success) { ResultObj = deviceModifyDTO });
        }
        [HttpGet("UserId")]
        public async Task<IActionResult> getDevicesForUserId(Guid UserId)
        {
            var devices = await _devicesService.FindAllDevicesByUserId(UserId);
            List<DeviceModifyDTO> deviceModifyDTO = new List<DeviceModifyDTO>();
            _mapper.Map(devices, deviceModifyDTO);
            return Ok(new ResponseCtrMsg<List<DeviceModifyDTO>>(CtrResult.Success) { ResultObj = deviceModifyDTO });
        }
        [HttpGet("ProjectId")]
        public async Task<IActionResult> getDevicesForProjectId(int projectId)
        {
            var devices = await _devicesService.FindAllDevicesByProjectId(projectId);
            List<DeviceModifyDTO> deviceModifyDTO = new List<DeviceModifyDTO>();
            _mapper.Map(devices, deviceModifyDTO);
            return Ok(new ResponseCtrMsg<List<DeviceModifyDTO>>(CtrResult.Success) { ResultObj = deviceModifyDTO });
        }

        [HttpPost]
        public async Task<IActionResult> addDevice(DeviceModifyDTO deviceModifyDTO)
        {
            if (ModelState.IsValid)
            {
                bool flag = await _devicesService.InsertDevice(deviceModifyDTO);
                if (flag)
                {
                    return Ok(new ResponseCtrMsg<TDevices>(CtrResult.Success) { });
                }
            }
            return BadRequest(new ResponseCtrMsg<TDevices>(CtrResult.Failure) { });
        }

        [HttpDelete("{deviceId}")]
        public async Task<IActionResult> removeDevice(int deviceId)
        {
            bool flag = await _devicesService.RemoveDevice(deviceId);
            if (flag)
            {
                return Ok(new ResponseCtrMsg<TDevices>(CtrResult.Success) { });
            }
            return BadRequest(new ResponseCtrMsg<TDevices>(CtrResult.Failure) { });
        }

        [HttpPut()]
        public async Task<IActionResult> updateDevice(TDevices device)
        {
            bool flag = await _devicesService.UpdateDevice(device);
            if (flag)
            {
                return Ok(new ResponseCtrMsg<TDevices>(CtrResult.Success) { });
            }
            return BadRequest(new ResponseCtrMsg<TDevices>(CtrResult.Failure) { });
        }
    }
}
