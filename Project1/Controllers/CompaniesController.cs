using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project1.Models;

namespace Project1.Controllers
{
    public class CompaniesController : Controller
    {
        private webappM4Entities db = new webappM4Entities();

        // GET: Companies
        public ActionResult Index()
        {
            return View(db.tbCompanies.ToList());
        }

        // GET: Companies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbCompany tbCompany = db.tbCompanies.Find(id);
            if (tbCompany == null)
            {
                return HttpNotFound();
            }
            return View(tbCompany);
        }

        // GET: Companies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "compID,compName,compRep,compDOE,compAddr,compCat,compEmail,compContact,compDesc")] tbCompany tbCompany)
        {
            if (ModelState.IsValid)
            {
                db.tbCompanies.Add(tbCompany);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbCompany);
        }

        // GET: Companies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbCompany tbCompany = db.tbCompanies.Find(id);
            if (tbCompany == null)
            {
                return HttpNotFound();
            }
            return View(tbCompany);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "compID,compName,compRep,compDOE,compAddr,compCat,compEmail,compContact,compDesc")] tbCompany tbCompany)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbCompany).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbCompany);
        }

        // GET: Companies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbCompany tbCompany = db.tbCompanies.Find(id);
            if (tbCompany == null)
            {
                return HttpNotFound();
            }
            return View(tbCompany);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbCompany tbCompany = db.tbCompanies.Find(id);
            db.tbCompanies.Remove(tbCompany);
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
