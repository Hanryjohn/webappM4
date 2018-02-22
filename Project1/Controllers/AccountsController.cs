using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project1.Models;

namespace Project1.Controllers
{
    /*
    public class CustomAuthorize : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }
        public bool OnAuthorization(UserAccount u)
        {
            if (u.UserRole.Equals("admin"))
                return true;
            else return false;
        }
    }
    */
    public class AccountsController : Controller
    {        
        // GET: Accounts
        public ActionResult Index(string searchString)
        {
            if (Session["UserId"] != null && Session["UserRole"].ToString() == "admin"){
                ViewBag.Message = TempData["Message"];
                ViewBag.Status = TempData["Status"];

                WebAppEntities db = new WebAppEntities();
                var users = from u in db.UserAccounts select u;

                if (!String.IsNullOrEmpty(searchString))
                {
                    users = users.Where(a => a.UserName.Contains(searchString));
                }
                return View(users.ToList());
            }
            else {
                TempData["Message"] = "You don't have enough privilege to do that";
                TempData["Status"] = "warning";
                return RedirectToAction("Login");
            }
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(UserAccount acc)
        {
            if (ModelState.IsValid)
            {
                using (WebAppEntities db = new WebAppEntities())
                {
                    db.UserAccounts.Add(acc); // add useraccount
                    db.SaveChanges(); //save changes to database
                }
                ModelState.Clear();
                ViewBag.Message = acc.UserName + " " + "has successfully registered.";
                ViewBag.Status = "success";
            }
            return View();
        }
        
        public ActionResult Login()
        {
            if (Session["UserId"] == null)
            {
                ViewBag.Message = TempData["Message"];
                ViewBag.Status = TempData["Status"];
                return View();
            }
            else
            {
                TempData["Message"] = "You're already logged in";
                TempData["Status"] = "warning";
                return RedirectToAction("Index");
            }
        }
        
        [HttpPost]
        public ActionResult Login(UserAccount acc)
        {
            using(WebAppEntities db = new WebAppEntities())
            {
                //var usr = db.UserAccounts.Where(u => u.UserName.CompareTo(acc.UserName) == 0 && u.UserPassword.CompareTo(acc.UserPassword) == 0).First();
                var usr = db.UserAccounts.Where(u => u.UserName.Equals(acc.UserName) && u.UserPassword.Equals(acc.UserPassword)).FirstOrDefault(); //for case sensitive check, change collation for column in database to Latin1_General_CS_AS 
                if (usr != null)
                {
                    Session["UserId"] = usr.UserId.ToString();
                    Session["UserName"] = usr.UserName.ToString();
                    Session["UserRole"] = usr.UserRole.ToString();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Either username or password is wrong");
                }
            }
            return View();
        }
        /*
        public ActionResult LoggedIn()
        {
            if(Session["UserId"] != null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        */
        public ActionResult LoggedOut()
        {
            if(Session["UserId"] == null || Session["UserName"] == null)
            {
                TempData["Message"] = "You can't perform that action.";
                TempData["Status"] = "warning";
                //ViewBag.Message = "You can't perform that action.";
                //ViewBag.Status = "warning";
                return RedirectToAction("Login");
            }
            else
            {
                Session["UserId"] = null;
                Session["UserName"] = null;
                TempData["Message"] = "You have successfully logged out.";
                TempData["Status"] = "info";
                //ViewBag.Message = "You have successfully logged out.";
                //ViewBag.Status = "info";
                return RedirectToAction("Login");
            }            
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
                ;
            UserAccount u = new WebAppEntities().UserAccounts.Find(id);
            if (u == null)
            {
                return HttpNotFound();
            }
            return View(u);
        }
        
        // GET: Movies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserAccount u = new WebAppEntities().UserAccounts.Find(id);
            if (u == null)
            {
                return HttpNotFound();
            }
            return View(u);
        }

        [HttpPost]
        public ActionResult Edit(UserAccount acc)
        {
            if (ModelState.IsValid)
            {
                WebAppEntities db = new WebAppEntities();
                db.Entry(acc).State = EntityState.Modified;
                db.SaveChanges();                
                ModelState.Clear();
                TempData["Message"] = acc.UserName + " " + "has been successfully edited.";
                TempData["Status"] = "success";
                return RedirectToAction("Index");
            }
            return View(acc);
        }
        
        // GET: Movies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserAccount u = new WebAppEntities().UserAccounts.Find(id);
            if (u == null)
            {
                return HttpNotFound();
            }
            return View(u);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WebAppEntities db = new WebAppEntities();
            UserAccount u = db.UserAccounts.Find(id);
            db.UserAccounts.Remove(u);
            db.SaveChanges();
            TempData["Message"] = u.UserName + " " + "has been successfully deleted.";
            TempData["Status"] = "success";
            return RedirectToAction("Index");
        }
        
    }
}