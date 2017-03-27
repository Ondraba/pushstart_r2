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
    public interface ISystemModul
    {
        IEnumerable<string> getWorkFlowCache();
        void newWorkFlowEvent(string workFlowEvent);
    }
}
