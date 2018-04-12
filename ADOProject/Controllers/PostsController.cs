using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ADOProject.Models;

namespace ADOProject.Controllers
{
    public class PostsController : Controller
    {
        private DbEntities db = new DbEntities();
        
        // GET: Posts
        public ActionResult Index()
        {
            return View(db.Posts.ToList());
        }

        [HttpGet]
        public ActionResult MyPosts()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var user = (User) Session["User"];

            return View("Index", db.Posts.Where(u => u.UserId.Equals(user.Id)).ToList());
    }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Body,UserId")] Post post)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            post.UserId = ((User)Session["User"]).Id;

            if (ModelState.IsValid)
            {
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(post);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);

            if (post == null)
            {
                return HttpNotFound();
            }

            if (Session["User"] == null || (((User)Session["User"]).Id != post.UserId && ((User)Session["User"]).Level == 0))
            {
                return RedirectToAction("Index", "Login");
            }

            
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Body")] Post post)
        {
            var user = (User) Session["User"];
            var dbPost = db.Posts.First(p => p.Id.Equals(post.Id));


            if (Session["User"] == null || (user.Id != dbPost.UserId && user.Level == 0))
            {
                return RedirectToAction("Index", "Login");
            }

            if (ModelState.IsValid)
            {
                dbPost.Body = post.Body;
                dbPost.Title = post.Title;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Post post = db.Posts.Find(id);
            
            if (post == null)
            {
                return HttpNotFound();
            }

            var user = (User)Session["User"];
            if (Session["User"] == null || (user.Id != post.UserId && user.Level == 0))
            {
                return RedirectToAction("Index", "Login");
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);

            var user = (User)Session["User"];
            if (Session["User"] == null || (user.Id != post.UserId && user.Level == 0))
            {
                return RedirectToAction("Index", "Login");
            }

            db.Posts.Remove(post);
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
