using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using GameDatabaseProject.Models;
using Microsoft.AspNet.Identity;
using System.IO;
using System.Net;

namespace GameDatabaseProject.Controllers
{
    public class AdminController : Controller
    {

        private DIC dic;
        private IMultimedia multimedia;
        private IGameRepository gameRepository;
        private IObjectRepository objectRepository;
        private IGenreRepository genreRepository;
        private IDeviceRepository deviceRepository;
        private IPublicUser userRepository;
        private IPublicGameRepository publicGameRepository;
        private Entities localDbContext;

        private ISystemModul systemModul;

        public AdminController()
        {
            this.dic = new DIC();
            this.gameRepository = dic.getGameRepository();
            this.objectRepository = dic.getObjectRepository();
            this.genreRepository = dic.getGenreRepository();
            this.deviceRepository = dic.getDeviceRepository();
            this.userRepository = dic.getPublicUserRepository();
            this.localDbContext = dic.returnCurrentPublicConnection();
            this.multimedia = dic.getMultimedia();
            this.systemModul = dic.getSystemModul();
            this.publicGameRepository = dic.getPublicGameRepository();
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
                ViewBag.GenreList = new SelectList(this.localDbContext.Genre, "Idpander", "Name");
                ViewBag.DeviceList = new SelectList(this.localDbContext.Device, "Id", "Name");
                ViewBag.PictureDrop = new SelectList(this.localDbContext.Games, "Picture");
                return View();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                this.systemModul.newWorkFlowEvent(e.ToString());
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
                if (this.multimedia.multimediaContentValid(uploadPicture))
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

        public ActionResult GetAllUsers()
        {
            var usersCollection = (from u in this.userRepository.GetUsers()
                                   select u).ToList();
            return View(usersCollection);
        }


        public ActionResult EditGame(int? id)
        {
            
                ViewBag.GenreList = new SelectList(this.localDbContext.Genre, "Id", "Name");
                ViewBag.DeviceList = new SelectList(this.localDbContext.Device, "Id", "Name");
                ViewBag.PictureDrop = new SelectList(this.localDbContext.Games, "PictureEdit");

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var getGameById = this.gameRepository.getGameById(id);

                if (getGameById == null)
                {
                    return HttpNotFound();
                }
                return View(getGameById);
          
           
        }

        [HttpPost, ActionName("EditGame")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult EditGame(Games game, int id, HttpPostedFileBase uploadPictureEdit)
        {
            if (this.multimedia.multimediaContentValid(uploadPictureEdit))
            {
                var pictureName = Path.GetFileName(uploadPictureEdit.FileName);
                string filePath = Path.Combine(Server.MapPath("~/upload/images/"), pictureName);
                uploadPictureEdit.SaveAs(filePath);
                game.Picture = Url.Content("~/upload/images/" + pictureName).ToString();
            }
            if (uploadPictureEdit == null)
            {
                game.Picture = game.Picture;
            }

            this.gameRepository.updateGame(game);
            this.objectRepository.save();
            return RedirectToAction("Index");
        }


        [Authorize]
        public ActionResult DeleteGame(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var getGameById = this.gameRepository.getGameById(id);
                if (getGameById == null)
                {
                    return HttpNotFound();
                }
                return View(getGameById);
            }

            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return Redirect(Request.UrlReferrer.ToString());
            }

        }


        [HttpPost, ActionName("DeleteGame")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteGame(int id)
        {
            try
            {
                this.gameRepository.removeGameById(id);
                this.objectRepository.save();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return Redirect(Request.UrlReferrer.ToString());
            }
        }

        public ActionResult getAllProposedGames()
        {
            return View(gameRepository.GetProposedGames());
        }

        public ActionResult proposedGameCheckProcess(int? id)
        {
            ViewBag.GenreList = new SelectList(this.localDbContext.Genre, "Id", "Name");
            ViewBag.DeviceList = new SelectList(this.localDbContext.Device, "Id", "Name");
            ViewBag.PictureDrop = new SelectList(this.localDbContext.ProposedGames, "PictureEdit");
            ViewBag.StateList = new SelectList(this.localDbContext.ProposedStates, "Id", "State_cz");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var getProposedGameById = this.gameRepository.getProposedGameById(id);

            if (getProposedGameById == null)
            {
                return HttpNotFound();
            }
            return View(getProposedGameById);
        }




    }


}