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
using System.Diagnostics;

namespace GameDatabaseProject.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        Entities _entitiesUser = new Entities();

        public ActionResult Index()
        {
            try
            {
               
            var gameListUser = _entitiesUser.Games.OrderByDescending(r => r.RankOveral).ToList().Take(20);

                var badGamesList = _entitiesUser.Games.OrderBy(g => g.RankOveral).ToList().Take(4).Reverse();
                ViewBag.BadGames = badGamesList; //viewbagem se da v ramci jednoho modelu oddělit dva vysledky foreach cyklu v jednom view

                return View(gameListUser);
            }

            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return Redirect(Request.UrlReferrer.ToString());
            }

        }

        public ActionResult GetBadGames()
        {
            try
            {
                var badGamesList = _entitiesUser.Games.OrderBy(g => g.RankOveral).ToList().Take(4).Reverse();
                TempData["badGames"] = badGamesList;
                ViewBag.BadGames = TempData["badGames"];
                return View(badGamesList);
            }

            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return Redirect(Request.UrlReferrer.ToString());
            }
        }

        public ActionResult MyReviews()
        {
            try { 
            var currentUser = User.Identity.GetUserId();
            var reviewList = _entitiesUser.Reviews
                .Where(r => r.User_Id == currentUser).OrderByDescending(o => o.Created_At).ToList();

                var findMe = _entitiesUser.AspNetUsers.Find(currentUser);


                ViewBag.UserName = findMe.NickName;
                ViewBag.UserPoints = findMe.Points;
                ViewBag.ReviewNumber = reviewList.Count();


                foreach (Reviews item in reviewList)
            {
                var findGame = _entitiesUser.Games.Find(item.Game_Id);
                item.Temporary = findGame.Name;
                
            }


            return View(reviewList);
            }

            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return Redirect(Request.UrlReferrer.ToString());
            }
        }



        public ActionResult UsersReviews (string id) //jeste review nezná, není na ní, je v detailu ! nelze pouzit review automaticky
        {

            try
            {
                var findUser = _entitiesUser.AspNetUsers.Find(id); //dá se používat pro unikátní akci, ne foreach

                ViewBag.UserName = findUser.NickName; 
                ViewBag.UserPoints = findUser.Points;
             

                var getAllUserReview = _entitiesUser.Reviews
                    .Where(r => r.User_Id == findUser.Id);

                ViewBag.ReviewNumber = getAllUserReview.Count();


                foreach (Reviews item in getAllUserReview)
                {
                    var findGame = _entitiesUser.Games.Find(item.Game_Id);
                    item.Temporary = findGame.Name;  //reviews nemaji klíč v game.name, ale pouze game.id, musí se dočasně dosadit

                }


                return View(getAllUserReview);


            }

            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return Redirect(Request.UrlReferrer.ToString());
            }
        }


        [Authorize]
        public ActionResult EditMyReview(int? id, string redirectUrl)
        {
            try { 

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var findReview = _entitiesUser.Reviews.Find(id);

            if (findReview == null)
            {
                return HttpNotFound();
            }

            var currentUser = User.Identity.GetUserId();

            if (findReview.User_Id == currentUser)
            {

                return View(findReview);
            }

            return Redirect(Request.UrlReferrer.ToString());

            }

            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return Redirect(Request.UrlReferrer.ToString());
            }

        }



        [HttpPost, ActionName("EditMyReview")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult EditMyReview(Reviews review, int? id) //ví na které url review je
        {
            try { 
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            _entitiesUser.Entry(review).State = EntityState.Modified;

            var setGameReview = _entitiesUser.Reviews.Find(review.Id);
            var findGame = _entitiesUser.Games.Find(setGameReview.Game_Id);

            

          
            findGame.RankOveral = ((findGame.RankOveral * (findGame.RankCount - 1)) + review.UserRank) / findGame.RankCount;
            _entitiesUser.Entry(findGame).State = EntityState.Modified;
       

             _entitiesUser.SaveChanges();
            //ViewBag.Result = true;
            return RedirectToAction("MyReviews","User");

            }

            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return Redirect(Request.UrlReferrer.ToString());
            }
        }




        [Authorize]
        public ActionResult MyDeleteReview(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try { 
                        var currentUser = User.Identity.GetUserId();
                        var findReview = _entitiesUser.Reviews.Find(id);
                        var findGame = _entitiesUser.Games.Find(findReview.Game_Id);
                        if (findReview == null)
                        {
                            return HttpNotFound();
                        }

                        if (findReview.User_Id == currentUser)
                        {
              

                            findGame.RankCount -= 1;

                            // po dvojtou závorku je propočet současného průměru bez nového hodnocení
                            if (findGame.RankCount > 0)
                            {
                            findGame.RankOveral = ((findGame.RankOveral * (findGame.RankCount + 1))- findReview.UserRank) / (findGame.RankCount);
                            }

                            else
                            {
                                findGame.RankOveral = 0;
                                findGame.RankCount = 0;
                            }

                            _entitiesUser.Reviews.Remove(findReview);
                            _entitiesUser.Entry(findGame).State = EntityState.Modified;
                            _entitiesUser.SaveChanges();

                            _entitiesUser.SaveChanges();
                            return RedirectToAction("MyReviews", "User");

                            }
            }

                            catch (Exception e)
                            {
                                Console.WriteLine("{0} Exception caught.", e);
                            }

            return Redirect(Request.UrlReferrer.ToString());
        }


        //[HttpPost, ActionName("MyDeleteReview")]
        //[Authorize] //[Authorize(Roles = "Admin, Super User")] dá se autorizovat role
        //[ValidateAntiForgeryToken]
        //public ActionResult MyDeleteReview(int id, Reviews review) 
        //{
        //    var findReview = _entitiesUser.Reviews.Find(id);
        //    _entitiesUser.Reviews.Remove(findReview);
        //    _entitiesUser.SaveChanges();
        //    return RedirectToAction("MyReviews", "User"); 
        //}







        public ActionResult AllGames()
        {
            try {
                var allDistList = _entitiesUser.Distributors.ToList();
                var uniqDistributors= (from e in allDistList select e.Name).Select(x => x).Distinct(); //pro korenci duplicitních záznamů
               

               
                
                var allCreators = _entitiesUser.Creators.ToList();
                var uniqCreators = (from e in allCreators select e.Name).Select(x => x).Distinct();

                var getAllGenres = _entitiesUser.Genre.ToList();



                ViewBag.listGenres = new SelectList(_entitiesUser.Genre,"Name", "Name"); //staci name, id by dropdownlist vzal jako parametr a muselo by se porovnavat id na inputu dropdownlistu
                 //ViewBag.listGenres = new SelectList(new List<SelectListItem>(getGenre)); //pokud chci list na frontendu
              
                ViewBag.listDistributors = new SelectList(uniqDistributors);
                ViewBag.listCreators = new SelectList(uniqCreators);
                ViewBag.listDevices = new SelectList(_entitiesUser.Device, "Name","Name");

                var gameListUser = _entitiesUser.Games.OrderByDescending(r => r.RankOveral).ToList();
            return View(gameListUser);
            }

            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return Redirect(Request.UrlReferrer.ToString());
            }
        }


        //public ActionResult SortAllGames(string genreDrop, string distributorDrop, string creatorDrop, string deviceDrop, FormCollection formCollection)

        //{

        //    try { 
        //    var gameList = _entitiesUser.Games.ToList();
        //        var finalGameList = new List<Games>();
        //    Dictionary<string, string> dict = new Dictionary<string, string>();
        //    dict.Add("Genre", genreDrop);
        //    dict.Add("Creators", creatorDrop);
        //    dict.Add("Distributors", distributorDrop);
        //    dict.Add("Devices", deviceDrop);

        //  //  foreach (KeyValuePair<string, string> pair in dict)
        //  //  {
        //  //          if (!string.IsNullOrEmpty(pair.Value))
        //  //          {

        //  //              var searchCriterias = properties
        //  //.Select(p => CreateSearchCriteria(p.PropertyType, p.Name))
        //  //.Where(s => s != null)
        //  //.ToList();

        //  //          }

        //  //          return View(finalGameList);
        //  //          }
        //  //  }
        //  //      return View ("Index");

        //  //  }

        //    catch (Exception e)
        //    {
        //        Console.WriteLine("{0} Exception caught.", e);
        //        return Redirect(Request.UrlReferrer.ToString());
        //    }
        //}


        public ActionResult TestDetailList(int id)
        {
            try { 
            var findDetail = _entitiesUser.Games.Find(id);

            var reviewList = _entitiesUser.Reviews
                .Where(g => g.Game_Id == findDetail.Id).ToList();
            return View(reviewList);
            }

            catch(Exception)
            {
                
                return View("Index");
            }
        }



        public ActionResult SearchResult(string searchString)
        {

            try { 
                    if (!String.IsNullOrEmpty(searchString))
                    {

                        var gameListUser = _entitiesUser.Games.Where(s => s.Name.Contains(searchString)).ToList();
                        return View(gameListUser);
                    }

                    if (String.IsNullOrEmpty(searchString))
                        {
                        return Redirect(Request.UrlReferrer.ToString()); //vrací url příchodu
                        }
                 }

            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return Redirect(Request.UrlReferrer.ToString());
            }
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult GetUsers()
        {

            try { 
            var aspUserList = _entitiesUser.AspNetUsers.OrderByDescending(p => p.Points).ToList();

            return View(aspUserList);
            }

            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return Redirect(Request.UrlReferrer.ToString());
            }
        }

        //public ActionResult SearchBasic(string searchString)
        //{
        //    var findAllGames = from g in _entitiesUser.Games
        //                       select g;
        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        findAllGames = findAllGames.Where(s => s.Name.Contains(searchString));
        //    }

        //    return View(findAllGames);
        //}


        public ActionResult Detail(int id)
        {
            try {
                var findDetail = _entitiesUser.Games.Find(id);




                var currentUser = User.Identity.GetUserId();
                var GetCurrentUser = _entitiesUser.AspNetUsers.Find(currentUser);


                var reviewList = _entitiesUser.Reviews
                    .Where(r => r.Game_Id == findDetail.Id).OrderByDescending(p => p.ReviewPoints).ToList();

                foreach (Reviews item in reviewList)  //nutno iterovat vsechny reviewslisty, pak najít vsechny usery (where) a nakonec vsem userum přepočítat body
                {


                    var findUser = _entitiesUser.AspNetUsers
                        .Where(u => u.Id == item.User_Id);

                    foreach (AspNetUsers user in findUser) {
                        item.ReviewPoints = user.Points;
                    }

                }



                ViewBag.reviewViewbagList = reviewList;  //udela z viewbagu list a ten se pak projde ve views foreach cyklem

                return View(findDetail);
            }

            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }

            return Redirect(Request.UrlReferrer.ToString());
        }





        [Authorize]
        public ActionResult CreateUserReview(int id, Games game)
        {
            try { 
                    var getCurrentUser = User.Identity.GetUserId();
                    var findUserInReview = _entitiesUser.Reviews
                        .Where(r => r.Game_Id == game.Id && r.User_Id == getCurrentUser).ToList();
                    if (findUserInReview.Count == 0)
                    {

                        ViewBag.UserList = new SelectList(_entitiesUser.AspNetUsers, "Id", "Email");
                        ViewBag.GameList = new SelectList(_entitiesUser.Games, "Id", "Name");



                        return View();
                     }
                }

            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }

            return Redirect(Request.UrlReferrer.ToString());
        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult CreateUserReview(AspNetUsers aspUser, Games game, Reviews review)
        {
            try { 
            if (ModelState.IsValid)
            {

                //POZOR
                //review se nedá prohledat anonymním intem, protože tady už neprobíhá náklik
                //post metoda zná instanci svého modelu (games)
                //hleda se v něm dá pomocí instance Games, ne pomocí parametru int, který říká jen to, že nově vytvořené id ve formuláře bude novým id nové instance modelu 
                // jiná id zase jen spáruje formulář s modelem > zobrazí edit nebo detail
                ViewBag.ShowUserList = new SelectList(_entitiesUser.AspNetUsers, "Id", "Email");

                review.Game_Id = game.Id; //není potřeba jí hledat pomocí find, viz url detailu > asp už ví na které je hře


                review.User_Id = User.Identity.GetUserId();



                //entitiesUser.Reviews
                //.Where(r => r.Game_Id == game.Id && r.User_Id == getCurrentUser).ToList();
                var getCurrentUserId = User.Identity.GetUserId();
                

                var findNickName = _entitiesUser.AspNetUsers.Find(getCurrentUserId);  //osetreni null hodnoty ke které nejde přidávat +=1

                if (findNickName.Points == null)
                {
                    findNickName.Points = 1;
                    review.ReviewPoints = findNickName.Points;
                }



                
                review.User_Name = findNickName.NickName;  //nedávat nikdy primo current Usera pres identity.user, ale najít ho v jeho tabulce
                review.Created_At = DateTime.Now;
                review.ReviewPoints = findNickName.Points;
                _entitiesUser.Reviews.Add(review);
                _entitiesUser.SaveChanges();

                //review zna, protoze ji prave vytvoril v get metodě, která si tahá v razoru data z MODELU !!! razor je klíč, každá metoda ví k jakému modelu patří
                var setGameReview = _entitiesUser.Reviews.Find(review.Id);//získává id review z review.id, tkerou právě vytvořil jako new id v šabloně - new id bez modelu, prostě anonymní id
                var findGame = _entitiesUser.Games.Find(setGameReview.Game_Id);//najde hru podle review
                

                //nejprve se vytvoří nová instance review, ktera si chytne ID hry na které je URL otevřena
                //následně si prohléda tuhle review podle id z detailu a najde game.id a odkaz na skutečnou instanci hry

                findGame.RankCount += 1;
                findGame.RankOveral = ((findGame.RankOveral * (findGame.RankCount - 1)) + review.UserRank) / findGame.RankCount;
                _entitiesUser.Entry(findGame).State = EntityState.Modified;
                _entitiesUser.SaveChanges();
            }
                }

            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }
            return RedirectToAction("Detail", "User", new { id = review.Game_Id });
        }

        [Authorize]
        public ActionResult EditUserReview(int? id, string redirectUrl)
        {
            
            try { 

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var findReview = _entitiesUser.Reviews.Find(id);

            if (findReview == null)
            {
                return HttpNotFound();
            }

            var currentUser = User.Identity.GetUserId();

            if (findReview.User_Id == currentUser)
            {

                return View(findReview);
            }

            }

            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }

            return Redirect(Request.UrlReferrer.ToString());
        }



        [HttpPost, ActionName("EditUserReview")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult EditUserReview(Reviews review, int id) //ví na které url review je
        {

            try { 
           
            _entitiesUser.Entry(review).State = EntityState.Modified;
            _entitiesUser.SaveChanges();
            //ViewBag.Result = true;
            return RedirectToAction("Detail", "User", new { id = review.Game_Id });  //url je tvorena user/detail/ID třídy kterou detail vrací (games) z review.game
            }

            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return Redirect(Request.UrlReferrer.ToString());
            }
        }



        [Authorize]
        public ActionResult DeleteReview(int? id)
        {

            try { 
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var currentUser = User.Identity.GetUserId();
                var findReview = _entitiesUser.Reviews.Find(id);
                if (findReview == null)
                {
                    return HttpNotFound();
                }

                if (findReview.User_Id == currentUser)
                {
                    return View(findReview);
                }
                return Redirect(Request.UrlReferrer.ToString());
              }

             catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return Redirect(Request.UrlReferrer.ToString());
            }
        
    }
    


        [HttpPost, ActionName("DeleteReview")]
        [Authorize] //[Authorize(Roles = "Admin, Super User")] dá se autorizovat role
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Reviews review) //automaticky zná svou review
        {
            try { 
            var findReview = _entitiesUser.Reviews.Find(id);
            _entitiesUser.Reviews.Remove(findReview);
            _entitiesUser.SaveChanges();
            return RedirectToAction("Detail", "User", new { id = findReview.Game_Id }); //url hry se musí zachytit dřív, nežli se smaže recenze

            }

            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return Redirect(Request.UrlReferrer.ToString());
            }
        }





        [Authorize]
        public ActionResult Like(int id, FavouriteReviews favReview)
        {

            try {
            if (ModelState.IsValid)
            {


                var getReviewId = _entitiesUser.Reviews.Find(id);
                string userID = getReviewId.User_Id;  //získám ID uživatele který napsal review
                int reviewID = getReviewId.Id;
                var getUserId = _entitiesUser.AspNetUsers.Find(userID);  //projdu uživatele, který má id totožné s výstavcem
                var getCurrentUser = User.Identity.GetUserId();  //ziskám aktuálního usera



                var activeUser = _entitiesUser.FavouriteReviews
                    .Where(u => u.FavouriteReviewId == reviewID && u.UserId == getCurrentUser).ToList();  
                //začne hledat záznam v tabulce favouriteReviews, který má ZÁROVEŇ totožné ID review i uživatele s daty získanými z review výše a vrátí je do listu
                //include by vyfiltrovalo jen urcity typ například inclue(m  => m.UserRank).where(u =>userRank > 50)



                if (activeUser.Count() == 0 && getReviewId.User_Id != getCurrentUser)  //pokud je list favourite uživatelů roven 0
                {
                    getUserId.Points += 1;  //nalezený uživatel, kterýá napsal recenzi
                    getReviewId.ReviewPoints = getUserId.Points;

                    favReview.UserId = getCurrentUser;
                    favReview.FavouriteReviewId = reviewID;
                    _entitiesUser.FavouriteReviews.Add(favReview);

                    _entitiesUser.Entry(getUserId).State = EntityState.Modified;
                    _entitiesUser.SaveChanges();

                  return  Redirect(Request.UrlReferrer.ToString());
                  
                  
                }
            }

            return Redirect(Request.UrlReferrer.ToString());

            }

            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return Redirect(Request.UrlReferrer.ToString());
            }
        }




        public ActionResult PartialReviewToList()
        {
            try { 
            var reviewsToList= _entitiesUser.Reviews.ToList();
            return View(reviewsToList);
            }

            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return Redirect(Request.UrlReferrer.ToString());
            }
        }



    }



}