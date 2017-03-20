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
        private static ConnDIC instance = null;
        public Entities returnCurrentConnection()
        {
            Entities currentDbContext = new Entities();
            return currentDbContext;
        }

        public static ConnDIC returnInstance()
        {
            if (instance == null)
                instance = new ConnDIC();
            return instance;
        }
    }
}