using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameDatabaseProject.Models
{
    public class Multimedia : IMultimedia
    {
        private Entities currentDbContext;

        public Multimedia(Entities currentDbContext)
        {
            this.currentDbContext = currentDbContext;
        }

        private Entities getCurrentDbContext()
        {
            return this.currentDbContext;
        }
        public bool multimediaContentValid(HttpPostedFileBase postedFileBase)
        {
            if (postedFileBase != null && postedFileBase.ContentLength > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }




    }
}