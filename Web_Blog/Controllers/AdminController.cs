using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Web_Blog.Models;


namespace Web_Blog.Controllers
{
    public class AdminController : Controller
    {
        blog_Db db = new blog_Db();
        // GET: Admin
        public ActionResult Index()
        {
            ViewBag.MakaleSayisi = db.Makales.Count();
            ViewBag.YorumSayisi = db.Yorums.Count();

            return View();
        }

        public ActionResult Makale_Listele()
        {
            var makales = db.Makales.ToList();

            return View(makales);
        }   
        

        public ActionResult Makale_Ekle()
        {   // Sayfa ilk acılırken olan ıslemler burada tanımlanır.

            ViewBag.Kategori_Id = new SelectList(db.Kategoris, "Kategori_Id", "Kategori_Adi");
            return View();
        }
        [HttpPost]
        public ActionResult Makale_Ekle(Makale makale, string etiketler, HttpPostedFileBase Foto)
        {       //Makale Ekle ,Butonunna tıklandıgında olusacak ıslemler

            if (ModelState.IsValid)
            {

                if (Foto != null)
                {
                    WebImage img = new WebImage(Foto.InputStream);
                    FileInfo Fotoinfo = new FileInfo(Foto.FileName);
                    string newfoto = Guid.NewGuid().ToString() + Fotoinfo.Extension;
                    img.Resize(800, 350);
                    img.Save("~/Uploads/MakaleFoto/" + newfoto);
                    makale.Foto = "/Uploads/MakaleFoto/" + newfoto;



                }

                if (etiketler != null)
                {
                    string[] etiketdizi = etiketler.Split(',');
                    foreach (var i in etiketdizi)
                    {
                        var yenietiket = new Etiket { Etiket_Adi = i };
                        db.Etikets.Add(yenietiket);
                        makale.Etikets.Add(yenietiket);
                    }
                }               
                makale.Uye_id =Convert.ToInt32(Session["Uye_Id"]);
                db.Makales.Add(makale);
                db.SaveChanges();

                return RedirectToAction("Makale_Listele");
            } 

            return View();
        }
         
        

        public ActionResult Makale_Edit(int id)
        {
            var makale = db.Makales.Where(m => m.Makale_Id == id).SingleOrDefault();
            if (makale == null)
            {
                return HttpNotFound();
            }
            ViewBag.Kategori_Id = new SelectList(db.Kategoris, "Kategori_Id","Kategori_Adi",makale.Kategori_Id);

            return View(makale);
        }
        [HttpPost]  
        public ActionResult Makale_Edit(int id, HttpPostedFileBase Foto, Makale makale)
        {
            var makales = db.Makales.Where(m => m.Makale_Id == id).SingleOrDefault();
            if (ModelState.IsValid)
            {
               
                if (Foto != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(makales.Foto)))
                    {
                        System.IO.File.Delete(Server.MapPath(makales.Foto));
                    }
                    WebImage img = new WebImage(Foto.InputStream);
                    FileInfo Fotoinfo = new FileInfo(Foto.FileName);
                    string newfoto = Guid.NewGuid().ToString() + Fotoinfo.Extension;
                    img.Resize(800, 350);
                    img.Save("~/Uploads/MakaleFoto/" + newfoto);
                    makales.Foto = "/Uploads/MakaleFoto/" + newfoto;
                }
                    makales.Baslik = makale.Baslik;
                    makales.Icerik = makale.Icerik;
                    makales.Kategori_Id = makale.Kategori_Id;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                             
       
            {
                return View();
            }
        }


        public ActionResult Makale_Sil(int id)
        {
            var makale = db.Makales.Where(m => m.Makale_Id == id).SingleOrDefault();
            if (makale==null)
            {
                return HttpNotFound();

            }

            return View(makale);
        }
        [HttpPost]
        public ActionResult Makale_Sil(int id, FormCollection collection)
        {
            try
            {

                var makales = db.Makales.Where(m => m.Makale_Id == id).SingleOrDefault();
                if (makales == null)
                {
                    return HttpNotFound();
                }
                if (System.IO.File.Exists(Server.MapPath(makales.Foto)))
                {
                    System.IO.File.Delete(Server.MapPath(makales.Foto));
                }
                foreach (var item in makales.Yorums.ToList())
                {
                    db.Yorums.Remove(item);
                }
                foreach(var item in makales.Etikets.ToList())
                {
                    db.Etikets.Remove(item);
                }
                db.Makales.Remove(makales);
                db.SaveChanges();



                return RedirectToAction("Makale_Listele");
            }
            catch
            {
                return View();
            }
        }


    }
}