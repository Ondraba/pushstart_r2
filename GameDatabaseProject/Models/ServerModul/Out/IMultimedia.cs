using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameDatabaseProject.Models;
using System.IO;
using System.Web;

namespace GameDatabaseProject.Models
{
    public interface IMultimedia 
    {
        bool multimediaContentValid(HttpPostedFileBase postedFileBase);
    }
}
