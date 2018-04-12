using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using ADOProject.Models;
using ADOProject.ViewModels;

namespace ADOProject.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var db = new DbEntities();

                var usr = db.Users.FirstOrDefault(u => u.Username.Equals(model.Username));

                if (usr != null)
                {
                    var pwd = model.Password + usr.CreatedAt;

                    if (Crypto.VerifyHashedPassword(usr.Password, pwd))
                    {
                        Session["User"] = usr;
                        return Redirect("/");
                    }

                }

                ViewData["Message"] = "Username or password incorrect";
            }
            
            return View(model);
        }
        
        [HttpGet]
        public ActionResult Logout()
        {
            Session["User"] = null;
            return Redirect("/");
        }
    }
}