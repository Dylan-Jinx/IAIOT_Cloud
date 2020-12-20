using IAIOT_alpha_0_1_1.Models;
using IAIOT_alpha_0_1_1.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IAIOT_alpha_0_1_1.Services
{
    public interface ISensorService<T>
    {
        Task<List<T>> FindAllSensorByUserId(Guid userId);
        Task<List<T>> FindAllSensorsByProjectId(int projectId);
        Task<List<T>> FindAllSensorsByDevicetId(int deviceId);
        Task<T> FindSensorBySensorId(int sensorId);
        Task<bool> InsertSensor(SensorModifyDTO sensorModifyDTO);
        Task<bool> RemoveSensor(int sensorId);
        Task<bool> UpdateSensor(T sensor);
    }
}
