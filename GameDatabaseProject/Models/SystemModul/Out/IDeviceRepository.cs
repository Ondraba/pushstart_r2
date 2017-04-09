using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameDatabaseProject.Models;
using System.Data.Entity;

namespace GameDatabaseProject.Models
{
    public interface IDeviceRepository
    {
        IEnumerable<Device> getDevices();
        Device getDeviceById(int id);
        void addDevice(Device device);
        void removeDevice(int id);
        void updateDevice(Device device);
    }
}
