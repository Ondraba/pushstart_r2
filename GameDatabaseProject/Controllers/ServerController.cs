using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using GameDatabaseProject.Models;
using Microsoft.AspNet.Identity;
using System.IO;

namespace GameDatabaseProject.Controllers
{
    public class ServerController : Controller
    {
        private DIC dic;
        private IGameRepository gameRepository;
        private IObjectRepository objectRepository;
        private IGenreRepository genreRepository;
        private IDeviceRepository deviceRepository;
        private IServerUser userRepository;
        private Entities localDbContext;

        private ISystemModul systemModul;

        public ServerController()
        {
            this.dic = new DIC();
            this.gameRepository = dic.getGameRepository();
            this.objectRepository = dic.getObjectRepository();
            this.genreRepository = dic.getGenreRepository();
            this.deviceRepository = dic.getDeviceRepository();
            this.userRepository = dic.getServerUserRepository();
            this.localDbContext = dic.returnCurrentPublicConnection();
            this.systemModul = dic.getSystemModul();
        }
        // GET: Server
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAllUsers()
        {
            return View(userRepository.GetUsers());
        }

        public ActionResult CachedEventLog()
        {
            return View (this.systemModul.getWorkFlowCache());
        }

    }
}