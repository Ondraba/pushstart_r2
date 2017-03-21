using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameDatabaseProject.Models;
using System.Data.Entity;

namespace GameDatabaseProject.Models
{
    public class DeviceRepository : IDeviceRepository
    {
        private Entities currentDbContext;
       

        public DeviceRepository(Entities currentDbContext)
        {
            this.currentDbContext = currentDbContext;
            
        }

        private Entities getCurrentDbContext()
        {
            return this.currentDbContext;
        }

        public IEnumerable<Device> getDevices()
        {
            return getCurrentDbContext().Device.ToList();
        }

        public void addDevice(Device device)
        {
            getCurrentDbContext().Device.Add(device);
        }

        public Device getDeviceById(int id)
        {
            return getCurrentDbContext().Device.Find(id);
        }

        public void removeDevice(int id)
        {
            Device deviceToremove = getCurrentDbContext().Device.Find(id);
            getCurrentDbContext().Device.Remove(deviceToremove);
        }

        public void updateDevice(Device device)
        {
            getCurrentDbContext().Entry(device).State = EntityState.Modified;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    getCurrentDbContext().Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }



    }
}