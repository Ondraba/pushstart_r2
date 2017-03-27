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
    public class SystemModul : ISystemModul
    {
        private Entities currentDbContext;
        private List<string> workFlowCache = new List<string>();

        public SystemModul(Entities currentDbContext)
        {
            this.currentDbContext = currentDbContext;
        }

        public IEnumerable<string> getWorkFlowCache()
        {
            return this.workFlowCache;
        }

        public void newWorkFlowEvent(string workFlowEvent)
        {
            this.workFlowCache.Add(workFlowEvent);
        }
    }
}