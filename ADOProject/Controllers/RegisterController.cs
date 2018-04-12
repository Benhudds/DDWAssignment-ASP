using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using ADOProject.Models;
using ADOProject.ViewModels;

namespace ADOProject.Controllers
{
    public class RegisterController : Controller
    {
        // GET: register
        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs("Get", "Post")]
        public ActionResult EmailAvailable(string email)
        {
            var db = new DbEntities();
            if (db.Users.Any(u => u.EmailAddress.Equals(email)))
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs("Get", "Post")]
        public ActionResult UsernameAvailable(string username)
        {
            var db = new DbEntities();
            if (db.Users.Any(u => u.Username.Equals(username)))
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var db = new DbEntities();

                var createdAt = DateTime.Now.ToString(CultureInfo.InvariantCulture);

                var pwd = model.Password + createdAt;

                db.Users.Add(new User
                {
                    Username = model.Username,
                    EmailAddress = model.Email,
                    Password = Crypto.HashPassword(pwd),
                    CreatedAt = createdAt
                });

                db.SaveChanges();

                var usr = db.Users.First(u => u.Username.Equals(model.Username));


                Session["User"] = usr;

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

    }
}