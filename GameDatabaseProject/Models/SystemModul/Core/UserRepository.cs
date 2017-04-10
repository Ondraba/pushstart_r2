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

namespace GameDatabaseProject.Models
{
    public class UserRepository : IPublicUser, IServerUser
    {
        private Entities currentDbContext;

            public UserRepository(Entities currentDbContext)
            {
                this.currentDbContext = currentDbContext;
            }

            private Entities getCurrentDbContext()
            {
                return this.currentDbContext;
            }

            public IEnumerable<AspNetUsers> GetUsers()
            {
                return getCurrentDbContext().AspNetUsers.ToList();
            }

            public AspNetUsers geUserById(string id)
            {
                return getCurrentDbContext().AspNetUsers.Find(id);
            }

            public void removeUser(string id)
            {
                AspNetUsers userToRemove = currentDbContext.AspNetUsers.Find(id);
                getCurrentDbContext().AspNetUsers.Remove(userToRemove);
            }

            public int getUsersCount()
            {
                return this.GetUsers().Count();
            }

            public int getOnlineUsersCount()
            {
                var onlineUsersCount = getOnlineUsersCount();
                return onlineUsersCount;
            }
            
            public IEnumerable<string> getAllUsersMailList()
            {
                var usersMailList = (from m in this.getCurrentDbContext().AspNetUsers
                                   select m.Email).ToList();
                return usersMailList;
            }

            public AspNetUsers getCurrentActiveUser(string userId)
            {
                 return geUserById(userId);
            }
    }
    }
