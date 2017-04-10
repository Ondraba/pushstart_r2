using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameDatabaseProject.Models;
using System.Data.Entity;

namespace GameDatabaseProject.Models
{
    public interface IPublicUser
    {
        IEnumerable<AspNetUsers> GetUsers();
        AspNetUsers geUserById(string id);
        void removeUser(string id);
        int getOnlineUsersCount();
        AspNetUsers getCurrentActiveUser(string userId);
    }
}
