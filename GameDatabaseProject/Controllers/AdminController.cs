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
        private IGameRepository gameRepository;
        private IObjectRepository objectRepository;

        public AdminController()
        {
            this.dic = new DIC();
            this.gameRepository = dic.getGameRepository();
            this.objectRepository = dic.getObjectRepository();
        }

        // GET: Admin
        public ActionResult Index()
        {
            var gameCollection = (from g in this.gameRepository.GetGames()
                                 select g).ToList();
            return View(gameCollection);
        }

        public void getUser()
        {
            var user = User.Identity.GetUserId();

        }
    }
}