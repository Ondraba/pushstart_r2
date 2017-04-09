using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using GameDatabaseProject.Models;
using Microsoft.AspNet.Identity;
using System.IO;
using System.Net;



namespace GameDatabaseProject.Models
{
    public class ObjectRepository : IObjectRepository
    {
        private Entities currentDbContext;
    
        public ObjectRepository(Entities currentDbContext)
        {
            this.currentDbContext = currentDbContext;
        }

        private Entities getCurrentDbContext()
        {
            return this.currentDbContext;
        }

        public void save()
        {
            this.getCurrentDbContext().SaveChanges();
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