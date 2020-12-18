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
    public class DevicesService : IDevicesService<TDevices>
    {
        private readonly ILogger<DevicesService> _logger;
        private readonly IServerAddressesFeature _serverAddresses;
        private readonly IAuthenticationService _authenticationService;
        private readonly IAIOTCloudContext _context;
        private readonly IMapper _mapper;
        public DevicesService
            (
            IAIOTCloudContext context,
            IServerAddressesFeature serverAddresses,
            IAuthenticationService authenticationService,
            ILogger<DevicesService> logger,
            IMapper mapper
            )
        {
            _logger = logger;
            _context = context;
            _serverAddresses = serverAddresses;
            _authenticationService = authenticationService;
            _mapper = mapper;
        }

        public async Task<List<TDevices>> FindAllDevicesByProjectId(int projectId)
        {
            var devices =  await _context.TDevices.Where(a => a.ProjectId == projectId).ToListAsync();
            return devices;
        }

        public async Task<List<TDevices>> FindAllDevicesByUserId(Guid userId)
        {
            var devices = await _context.TDevices.Where(a => a.CtrUserId == userId).ToListAsync();
            return devices;
        }

        public async Task<TDevices> FindDeviceByDeviceId(int deviceId)
        {
            var device = await _context.TDevices.FindAsync(deviceId);
            return device;
        }

        public async Task<bool> InsertDevice(DeviceModifyDTO deviceModifyDTO)
        {
            TDevices device = new TDevices();
            _mapper.Map(deviceModifyDTO, device);
            device.CreateDate = DateTime.Now;
            await _context.TDevices.AddAsync(device);
            bool flag = await _context.SaveChangesAsync() > 0;
            return flag;
        }

        public async Task<bool> RemoveDevice(int deviceId)
        {
            var device = await _context.TDevices.FindAsync(deviceId);
            _context.TDevices.Remove(device);
            bool flag = await _context.SaveChangesAsync() > 0;
            return flag;
        }

        public async Task<bool> UpdateDevice(TDevices device)
        {
            _context.TDevices.Update(device);
            bool flag = await _context.SaveChangesAsync() > 0;
            return flag;
        }
    }
}
