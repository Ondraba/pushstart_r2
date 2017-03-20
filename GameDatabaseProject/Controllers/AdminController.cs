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
    public class AdminController : Controller
    {
    
        private DIC dic;
        private IGameRepository gameRepository;
        private IObjectRepository objectRepository;
        private IGenreRepository genreRepository;
        private IDeviceRepository deviceRepository;
        private Entities localDbContext;

        public AdminController()
        {
            this.dic = new DIC();
            this.gameRepository = dic.getGameRepository();
            this.objectRepository = dic.getObjectRepository();
            this.genreRepository = dic.getGenreRepository();
            this.deviceRepository = dic.getDeviceRepository();
            this.localDbContext = dic.returnCurrentPublicConnection();
        }

        // GET: Admin
        public ActionResult Index()
        {
            var gameCollection = (from g in this.gameRepository.GetGames()
                                 select g).ToList();
            return View(gameCollection);
        }


        public ActionResult CreateGame()
        {
            try
            {
                ViewBag.GenreList = new SelectList(this.localDbContext.Genre, "Id", "Name");
                ViewBag.DeviceList = new SelectList(this.localDbContext.Device, "Id", "Name");
                ViewBag.PictureDrop = new SelectList(this.localDbContext.Games, "Picture");
                return View();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return Redirect(Request.UrlReferrer.ToString());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult CreateGame(Games games, HttpPostedFileBase uploadPicture)
        {
                if (ModelState.IsValid)
                {
                    if (uploadPicture != null && uploadPicture.ContentLength > 0)
                    {
                        var pictureName = Path.GetFileName(uploadPicture.FileName);
                        string filePath = Path.Combine(Server.MapPath("~/upload/images/"), pictureName);
                        uploadPicture.SaveAs(filePath);
                        games.Picture = Url.Content("~/upload/images/" + pictureName).ToString();
                    }
                    games.RankCount = 0;
                    games.RankOveral = 0;
                    games.Distributor_Id = 999;
                    games.Creator_Id = 999;
                    this.localDbContext.Games.Add(games);
                    this.localDbContext.SaveChanges();
                }
            return RedirectToAction("Index");
        }


    }
}