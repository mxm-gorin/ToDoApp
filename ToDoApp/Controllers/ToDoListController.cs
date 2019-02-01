using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    public class ToDoListController : Controller
    {
        private IEnumerable<ToDoItem> GetToDoList()
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            return db.ToDoList.ToList().Where(x => x.User == currentUser);
        }

        public ActionResult BuildToDoTable()
        {

            return PartialView("_ToDoTable", GetToDoList());
        }

        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDoItem toDoItem = db.ToDoList.Find(id);
            if (toDoItem == null)
            {
                return HttpNotFound();
            }
            return View(toDoItem);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description")] ToDoItem toDoItem)
        {
            if (ModelState.IsValid)
            {
                string currentUserId = User.Identity.GetUserId();
                ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
                toDoItem.User = currentUser;
                toDoItem.IsDone = false;
                db.ToDoList.Add(toDoItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(toDoItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AJAXCreate([Bind(Include = "Id,Description")] ToDoItem toDoItem)
        {
            if (ModelState.IsValid)
            {
                string currentUserId = User.Identity.GetUserId();
                ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
                toDoItem.User = currentUser;
                toDoItem.IsDone = false;
                db.ToDoList.Add(toDoItem);
                db.SaveChanges();
            }

            return PartialView("_ToDoTable", GetToDoList());
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDoItem toDoItem = db.ToDoList.Find(id);
            if (toDoItem == null)
            {
                return HttpNotFound();
            }
            return View(toDoItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description,IsDone")] ToDoItem toDoItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(toDoItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(toDoItem);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDoItem toDoItem = db.ToDoList.Find(id);
            if (toDoItem == null)
            {
                return HttpNotFound();
            }
            return View(toDoItem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ToDoItem toDoItem = db.ToDoList.Find(id);
            db.ToDoList.Remove(toDoItem);
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
