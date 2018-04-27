using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Web_Blog.Models;

namespace Web_Blog.Controllers
{
    public class AdminUyeController : Controller
    {
        private blog_Db db = new blog_Db();

       
        public ActionResult Index()
        {
            var uyes = db.Uyes.Include(u => u.Yetki);
            return View(uyes.OrderByDescending(u=>u.Uye_Id).ToList());
        }

      
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Uye uye = db.Uyes.Find(id);
            if (uye == null)
            {
                return HttpNotFound();
            }
            return View(uye);
        }

        
        public ActionResult Create()
        {
            ViewBag.Yetki_Id = new SelectList(db.Yetkis, "Yetki_Id", "Yetki1");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Uye_Id,Kullanici_Adi,Email,Sifre,AdSoyad,Foto,Yetki_Id")] Uye uye)
        {
            if (ModelState.IsValid)
            {
                db.Uyes.Add(uye);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Yetki_Id = new SelectList(db.Yetkis, "Yetki_Id", "Yetki1", uye.Yetki_Id);
            return View(uye);
        }

  
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Uye uye = db.Uyes.Find(id);
            if (uye == null)
            {
                return HttpNotFound();
            }
            ViewBag.Yetki_Id = new SelectList(db.Yetkis, "Yetki_Id", "Yetki1", uye.Yetki_Id);
            return View(uye);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Uye_Id,Kullanici_Adi,Email,Sifre,AdSoyad,Foto,Yetki_Id")] Uye uye)
        {
            if (ModelState.IsValid)
            {
                db.Entry(uye).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Yetki_Id = new SelectList(db.Yetkis, "Yetki_Id", "Yetki1", uye.Yetki_Id);
            return View(uye);
        }

        // GET: AdminUye/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Uye uye = db.Uyes.Find(id);
            if (uye == null)
            {
                return HttpNotFound();
            }
            return View(uye);
        }

        // POST: AdminUye/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Uye uye = db.Uyes.Find(id);
            db.Uyes.Remove(uye);
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
