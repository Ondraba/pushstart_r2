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

namespace GameDatabaseProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {



        Entities _entities = new Entities(); //defaulní entity 

        public ActionResult Index()
        {
            try { 
            var gameList = _entities.Games.OrderByDescending(r => r.RankOveral).ToList();
            return View(gameList);

        }
         catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return Redirect(Request.UrlReferrer.ToString());
            }
          }



        public ActionResult GetAllReviews()
        {
            try { 
            var reviewList = _entities.Reviews.OrderByDescending(y => y.Created_At).ToList();
            return View(reviewList);
            }

            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return Redirect(Request.UrlReferrer.ToString());
            }
        }

        public ActionResult DeleteReview(int? id)
        {
            try { 
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var findReview = _entities.Reviews.Find(id); //kvůli předvyplnění formuláře
            if (findReview == null)
            {
                return HttpNotFound();
            }
            return View(findReview);

            }

            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return Redirect(Request.UrlReferrer.ToString());
            }
        

    }


        [HttpPost, ActionName("DeleteReview")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteReview(int id)
        {

        try { 
            var findReview = _entities.Reviews.Find(id);
            _entities.Reviews.Remove(findReview);
            _entities.SaveChanges();
            return RedirectToAction("GetAllReviews");
            }

            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return Redirect(Request.UrlReferrer.ToString());
            }
        }



        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult CreateDistributor()
        {
           try { 
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
        public ActionResult CreateDistributor(Distributors distributor)
        {
            try { 
              if (ModelState.IsValid)
                {
                    _entities.Distributors.Add(distributor);
                _entities.SaveChanges();
                }
            return RedirectToAction("Index", "Home");

            }

            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return Redirect(Request.UrlReferrer.ToString());
            }

        }


        public ActionResult CreateCreator()
        {
            try { 
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
        public ActionResult CreateCreator(Creators creator)
        {
            try { 

            if (ModelState.IsValid)
            {
                _entities.Creators.Add(creator);
                _entities.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
            }

            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return Redirect(Request.UrlReferrer.ToString());
            }

        }






        public ActionResult Create()
        {
            try { 
            ViewBag.GenreList = new SelectList(_entities.Genre, "Id", "Name");
            ViewBag.DeviceList = new SelectList(_entities.Device, "Id", "Name");
            ViewBag.CreatorList = new SelectList(_entities.Creators, "Id", "Name");
            ViewBag.DistributorList = new SelectList(_entities.Distributors, "Id", "Name");
            ViewBag.PictureDrop = new SelectList(_entities.Games, "Picture");

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
        public ActionResult Create(Games games, HttpPostedFileBase uploadPicture)
        {

            try
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
                    _entities.Games.Add(games);
                    _entities.SaveChanges();
                }
            }

            catch(Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }

            return RedirectToAction("Index");
        }





        public ActionResult Edit(int? id)
        {

            try { 
            ViewBag.GenreList = new SelectList(_entities.Genre, "Id", "Name");
            ViewBag.CreatorList = new SelectList(_entities.Creators, "Id", "Name");
            ViewBag.DeviceList = new SelectList(_entities.Device, "Id", "Name");
            ViewBag.DistributorList = new SelectList(_entities.Distributors, "Id", "Name");
            ViewBag.PictureDrop = new SelectList(_entities.Games, "PictureEdit");


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var findGame = _entities.Games.Find(id);

            if (findGame == null)
            {
                return HttpNotFound();
            }

           

            return View(findGame);

            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return Redirect(Request.UrlReferrer.ToString());
            }


        }


        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(Games games, int id, HttpPostedFileBase uploadPictureEdit)
        {
            try { 
            if (ModelState.IsValid)
            {
               

                if (uploadPictureEdit != null && uploadPictureEdit.ContentLength > 0)
                {
                    var pictureName = Path.GetFileName(uploadPictureEdit.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/upload/images/"), pictureName);
                    uploadPictureEdit.SaveAs(filePath);
                    games.Picture = Url.Content("~/upload/images/" + pictureName).ToString();
                }

               
                if (uploadPictureEdit == null)
                {
                    games.Picture = games.Picture;
                }

           
                _entities.Entry(games).State = EntityState.Modified;
                _entities.SaveChanges();
                return RedirectToAction("Index");
            }

           

            return View(games);
            }

            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return Redirect(Request.UrlReferrer.ToString());
            }

        }


        [Authorize]
        public ActionResult Delete(int? id)
        {
            try { 
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var findGame = _entities.Games.Find(id); //kvůli předvyplnění formuláře
            if (findGame == null)
            {
                return HttpNotFound();
            }

           

            return View(findGame);
            }


            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return Redirect(Request.UrlReferrer.ToString());
            }

        }


        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try {
            var findGame = _entities.Games.Find(id);
            _entities.Games.Remove(findGame);
            _entities.SaveChanges();


        


            return RedirectToAction("Index");

            }

            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return Redirect(Request.UrlReferrer.ToString());
            }
        }




        public ActionResult Detail(int id)
        {

            try { 
            var findDetail = _entities.Games.Find(id);
            return View(findDetail);
            }

            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return Redirect(Request.UrlReferrer.ToString());
            }
        }
    


        [HttpPost]
        public ActionResult Detail(int id, Reviews newReview, Games game, AspNetUsers aspUser)
        {
           try { 

           if (ModelState.IsValid)
            {
                newReview.Game_Id = game.Id;  //sparuje id komentáře s id hry na kterou je aktivní detail formulář
                newReview.User_Id = User.Identity.GetUserId();
                newReview.Created_At = DateTime.Now;
                _entities.Reviews.Add(newReview);
                _entities.SaveChanges();
            }

            return Detail(id);
            }

            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return Redirect(Request.UrlReferrer.ToString());
            }
        }


       
        public void Like(int id, FavouriteReviews favReview)
        {
            try {
            if (ModelState.IsValid)
            {
                

                var getReviewId = _entities.Reviews.Find(id);
                string userID = getReviewId.User_Id;
                int reviewID = getReviewId.Id;
                var getUserId = _entities.AspNetUsers.Find(userID);
                var getCurrentUser = User.Identity.GetUserId();

               

                var activeUser = _entities.FavouriteReviews
                    .Where(u => u.FavouriteReviewId == reviewID && u.UserId == getCurrentUser).ToList();
                   

                    

                if (activeUser.Count() == 0)
                {
                    getUserId.Points +=1;

                    favReview.UserId = getCurrentUser;
                    favReview.FavouriteReviewId = reviewID;
                    _entities.FavouriteReviews.Add(favReview);

                    _entities.Entry(getUserId).State = EntityState.Modified;
                    _entities.SaveChanges();
                }

                

                
            }

            RedirectToAction("Index");

            }

            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}