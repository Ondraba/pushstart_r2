using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameDatabaseProject.Models
{
    public class ExperienceEngine
    {
        private Entities currentDbContext;
        private DIC dic;

        public ExperienceEngine(Entities currentDbContext)
        {
            this.currentDbContext = currentDbContext;
            this.dic = new DIC();
        }

        private Entities getCurrentDbContext()
        {
            return this.currentDbContext;
        }

        public bool userLevelCheck(AspNetUsers aspNetUser, int trustLevelNeeded)
        {
            var usersCurrentLevel = getCurrentDbContext().UserTrustLevels.Find(aspNetUser.TrustLevel_Id);
            if (usersCurrentLevel.System_Id >= trustLevelNeeded) { 
             return true;
            }
            else
            {
             return false;
            }
        }

    }
}