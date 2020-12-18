using IAIOT_alpha_0_1_1.Models;
using IAIOT_alpha_0_1_1.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IAIOT_alpha_0_1_1.Services
{
    public interface IDevicesService<T>
    {
        Task<List<T>> FindAllDevicesByUserId(Guid userId);
        Task<List<T>> FindAllDevicesByProjectId(int projectId);
        Task<T> FindDeviceByDeviceId(int deviceId);
        Task<bool> InsertDevice(DeviceModifyDTO deviceModifyDTO);
        Task<bool> RemoveDevice(int deviceId);
        Task<bool> UpdateDevice(TDevices device);
    }
}
