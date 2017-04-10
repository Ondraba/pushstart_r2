using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using GameDatabaseProject.Models;
using System.IO; //pimage paths, pictures
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System.Net; //httpstatus cody

namespace GameDatabaseProject.Models.ServerModul.Core
{
    public class AuthorityModul
    {
        private Entities currentDbContext;
        private DIC dic;

        public AuthorityModul(Entities currentDbContext)
        {
            this.currentDbContext = currentDbContext;
            this.dic = new DIC();
        }

        private Entities getCurrentDbContext()
        {
            return this.currentDbContext;
        }

        public bool userTrustLevelCheck(AspNetUsers aspNetUser, int trustLevelNeeded)
        {
            var usersCurrentLevel = getCurrentDbContext().UserTrustLevels.Find(aspNetUser.TrustLevel_Id);
            if (usersCurrentLevel.System_Id >= trustLevelNeeded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void trustLevelUpdate(AspNetUsers aspNetUser, int fkLevelValue)
        {
            aspNetUser.TrustLevel_Id = fkLevelValue;
            getCurrentDbContext().SaveChanges();
        }

        

    }
}