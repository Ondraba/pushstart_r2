using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameDatabaseProject.Models;
using System.Data.Entity;

namespace GameDatabaseProject.Models
{
    public class ConnDIC
    {
        public Entities returnCurrentConnection()
        {
            Entities currentDbContext = new Entities();
            return currentDbContext;
        }
    }
}