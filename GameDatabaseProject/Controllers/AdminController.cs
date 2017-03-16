using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using GameDatabaseProject.Models;
using Microsoft.AspNet.Identity;

namespace GameDatabaseProject.Controllers
{
    public class AdminController : Controller
    {
    
        private DIC dic;

        public AdminController()
        {
            this.dic = new DIC();
        }

        // GET: Admin
        public ActionResult Index()
        {
            var gameCollection = (from g in dic.getGameRepository().GetGames()
                                 select g).ToList();
            return View(gameCollection);
        }

        public void getUser()
        {
            var user = User.Identity.GetUserId();

        }
    }
}