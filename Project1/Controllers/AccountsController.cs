using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project1.Models;

namespace Project1.Controllers
{
    public class AccountsController : Controller
    {
        // GET: Accounts
        public ActionResult Index()
        {
            using (WebAppEntities db = new WebAppEntities())
                return View(db.UserAccounts.ToList());         
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
            ViewBag.Message = TempData["Message"];
            ViewBag.Status = TempData["Status"];
            
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserAccount acc)
        {
            using(WebAppEntities db = new WebAppEntities())
            {
                var usr = db.UserAccounts.Where(u => u.UserName == acc.UserName && u.UserPassword == acc.UserPassword).FirstOrDefault();
                if(usr != null)
                {
                    Session["UserId"] = usr.UserId.ToString();
                    Session["UserName"] = usr.UserName.ToString();
                    return RedirectToAction("LoggedIn");
                }
                else
                {
                    ModelState.AddModelError("", "Either username or password is wrong");
                }
            }
            return View();
        }

        public ActionResult LoggedIn()
        {
            if(Session["UserId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

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
    }
}