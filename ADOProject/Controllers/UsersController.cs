using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using ADOProject.Models;
using ADOProject.ViewModels;

namespace ADOProject.Controllers
{
    public class UsersController : Controller
    {
        private DbEntities db = new DbEntities();

        // GET: Users
        public ActionResult Index()
        {
            if (Session["User"] == null || ((User) Session["User"]).Level != 2)
            {
                return RedirectToAction("Index", "Login");
            }

            return View(db.Users.ToList());
        }

        // GET: Users/Details/
        public ActionResult Details()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var user = (User)Session["User"];
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            if (Session["User"] == null || (((User)Session["User"]).Level != 2 && ((User)Session["User"]).Id != user.Id))
            {
                return RedirectToAction("Index", "Login");
            }

            var viewModel = new EditUserViewModel()
            {
                Id = user.Id,
                Username = user.Username,
                CreatedAt = user.CreatedAt,
                Level = user.Level,
                EmailAddress = user.EmailAddress
            };


            return View(viewModel);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Username,CreatedAt,EmailAddress,Password,Level,ValidatePassword")] EditUserViewModel user)
        {
            if (Session["User"] == null || (((User)Session["User"]).Level != 2 && ((User)Session["User"]).Id != user.Id))
            {
                return RedirectToAction("Index", "Login");
            }

            var dbUser = db.Users.Find(user.Id);
            
            dbUser.Level = user.Level;
            if (user.Password != null)
            {
                dbUser.Password = Crypto.HashPassword(user.Password + dbUser.CreatedAt);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
            
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            if (Session["User"] == null || (((User)Session["User"]).Level != 2 && ((User)Session["User"]).Id != user.Id))
            {
                return RedirectToAction("Index", "Login");
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            if (Session["User"] == null || (((User)Session["User"]).Level != 2 && ((User)Session["User"]).Id != user.Id))
            {
                return RedirectToAction("Index", "Login");
            }

            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
