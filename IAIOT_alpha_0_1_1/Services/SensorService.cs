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
    public class SensorService : ISensorService<TSensors>
    {
        private readonly ILogger<SensorService> _logger;
        private readonly IServerAddressesFeature _serverAddresses;
        private readonly IAuthenticationService _authenticationService;
        private readonly IAIOTCloudContext _context;
        private readonly IMapper _mapper;
        public SensorService
            (
            IAIOTCloudContext context,
            IServerAddressesFeature serverAddresses,
            IAuthenticationService authenticationService,
            ILogger<SensorService> logger,
            IMapper mapper
            )
        {
            _logger = logger;
            _context = context;
            _serverAddresses = serverAddresses;
            _authenticationService = authenticationService;
            _mapper = mapper;
        }

        public async Task<List<TSensors>> FindAllSensorByUserId(Guid userId)
        {
            return await _context.TSensors.Where(a => a.CtrUserId == userId).ToListAsync();
        }

        public async Task<List<TSensors>> FindAllSensorsByProjectId(int projectId)
        {
            return await _context.TSensors.Where(a => a.ProjectId == projectId).ToListAsync();
        }

        public async Task<TSensors> FindSensorBySensorId(int sensorId)
        {
            return await _context.TSensors.FindAsync(sensorId);
        }

        public async Task<bool> InsertSensor(SensorModifyDTO sensorModifyDTO)
        {
            TSensors sensor = new TSensors();
            _mapper.Map(sensorModifyDTO, sensor);
            await _context.TSensors.AddAsync(sensor);
            bool flag = await _context.SaveChangesAsync() > 0;
            return flag;
        }

        public async Task<bool> RemoveSensor(int sensorId)
        {
            TSensors sensor = await _context.TSensors.FindAsync(sensorId);
            _context.TSensors.Remove(sensor);
            bool flag = await _context.SaveChangesAsync() > 0;
            return flag;
        }

        public async Task<bool> UpdateSensor(TSensors sensor)
        {
            _context.TSensors.Update(sensor);
            bool flag = await _context.SaveChangesAsync() > 0;
            return flag;
        }
    }
}
