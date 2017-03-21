using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDatabaseProject.Models
{
    public interface IServerUser
    {
        IEnumerable<AspNetUsers> GetUsers();
        int getUsersCount();
        int getOnlineUsersCount();
    }
}
