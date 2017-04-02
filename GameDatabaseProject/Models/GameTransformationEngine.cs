using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameDatabaseProject.Models;
using System.Data.Entity;

namespace GameDatabaseProject.Models
{
    public class GameTransformationEngine
    {
        private Entities currentDbContext;

        public  GameTransformationEngine(Entities currentDbContext)
        {
            this.currentDbContext = currentDbContext;
        }

       
    }
}