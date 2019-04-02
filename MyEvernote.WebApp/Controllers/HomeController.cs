using MyEvernote.BusinessLayer;
using MyEvernote.Entities;
using MyEvernote.Entities.Messages;
using MyEvernote.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyEvernote.WebApp.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            // --Category controller üzerinden gelen view talebi--
            //if(TempData["mm"] != null)
            //{
            //    return View(TempData["mm"] as List<Note>);
            //}

            NoteManager nm = new NoteManager();

            return View(nm.GetAllNote().OrderByDescending(x => x.ModifiedOn).ToList());
            //return View(nm.GetAllNoteQueryable().OrderByDescending(x=>x.ModifiedOn).ToList());
        }

        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CategoryManager cm = new CategoryManager();
            Category cat = cm.GetCategoryById(id.Value);

            if (cat == null)
            {
                return HttpNotFound();
            }
            return View("Index",cat.Notes.OrderByDescending(x=>x.ModifiedOn).ToList());
        }

        public ActionResult MostLiked()
        {
            NoteManager nm = new NoteManager();
            return View("Index",nm.GetAllNote().OrderByDescending(x => x.LikeCount).ToList());
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Login()
        {


            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            EvernoteUserManager eum = new EvernoteUserManager();
            BusinessLayerResult<EvernoteUser> res = eum.LoginUser(model); // Giriş Kontrolü

            if (ModelState.IsValid)
            {
                if (res.Errors.Count > 0)
                {
                    //Hataları ModelState'e ekleme..
                    
                    if(res.Errors.Find(x=>x.Code == ErrorMessageCode.UserIsNotActive) != null)
                    {
                        ViewBag.SetLink = "http://Home/Activate/1234-4894-65489";
                    }
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));

                    return View(model);
                }

                Session["login"] = res.Result; // Session'a kullanıcı bilgisi saklama..
                return RedirectToAction("Index"); // Giriş başarılıysa yönlendir.
            }

            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            EvernoteUserManager eum = new EvernoteUserManager();
            BusinessLayerResult<EvernoteUser> res = eum.RegisterUser(model);

            if (res.Errors.Count>0)
            {
                res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                return View(model);
            }
            
            return RedirectToAction("RegisterOk");
        }

        public ActionResult RegisterOk()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();

            return RedirectToAction("Index");
        }

        public ActionResult UserActivate(Guid activate_id)
        {
            // Burada kullanıcı aktivasyonu sağlanacak


            return View();
        }
    }

}