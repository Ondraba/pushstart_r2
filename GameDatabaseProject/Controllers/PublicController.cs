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
    public class PublicController : Controller
    {
        private PublicAcess dic = new DIC();
        private IMultimedia multimedia;
        private IPublicGameRepository gameRepository;
        private IObjectRepository objectRepository;
        private Entities localDbContext;
        private ISystemModul systemModul;
        private IPublicUser publicUser;

        public PublicController()
        {
            this.gameRepository = dic.getPublicGameRepository();
            this.objectRepository = dic.getObjectRepository();
            this.multimedia = dic.getMultimedia();
            this.localDbContext = dic.returnCurrentPublicConnection();
            this.systemModul = dic.getSystemModul();
            this.publicUser = dic.getPublicUserRepository();
        }

        // GET: Public
        public ActionResult Index()
        {
            var gameCollection = (from g in this.gameRepository.GetGames()
                                  select g).ToList();
            return View(gameCollection);
        }

        public ActionResult ProposeGame(int trustLevel = 2)
        {
                try
                {
                var ui = this.publicUser.getCurrentActiveUser(User.Identity.GetUserId()).TrustLevel_Id;
                    if (this.publicUser.getCurrentActiveUser(User.Identity.GetUserId()).TrustLevel_Id >= trustLevel)
                    {
                        ViewBag.GenreList = new SelectList(this.localDbContext.Genre, "Id", "Name");
                        ViewBag.DeviceList = new SelectList(this.localDbContext.Device, "Id", "Name");
                        ViewBag.PictureDrop = new SelectList(this.localDbContext.Games, "Picture");
                        ViewBag.StateList = new SelectList(this.localDbContext.ProposedStates, "Id", "Name");
                        return View();
                     }
                    else
                    {
                    this.systemModul.newWorkFlowEvent("Proposed game failed.");
                    Console.WriteLine("{0} Proposed game failed.");
                    return RedirectToAction("Index");
                }
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
        public ActionResult ProposeGame(AspNetUsers aspUser, ProposedGames proposedGame, HttpPostedFileBase uploadPicture)
        {
            if (ModelState.IsValid)
            {
                if (this.multimedia.multimediaContentValid(uploadPicture))
                {
                    var pictureName = Path.GetFileName(uploadPicture.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/upload/images/"), pictureName);
                    uploadPicture.SaveAs(filePath);
                    proposedGame.Picture = Url.Content("~/upload/images/" + pictureName).ToString();
                }
                proposedGame.RankCount = 0;
                proposedGame.RankOveral = 0;
                proposedGame.Distributor_Id = 999;
                proposedGame.Creator_Id = 999;
                proposedGame.State_Id = 1;
                proposedGame.ProposedBy = User.Identity.GetUserName();

                this.gameRepository.proposeNewGame(proposedGame);
                this.objectRepository.save();
            }
            return RedirectToAction("Index");
        }
    }
}