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
    public class KullaniciController : Controller
    {
        blog_Db db = new blog_Db();
        // GET: Kullanici
        public ActionResult Index(int id )
        {
            var kullanici = db.Uyes.Where(u => u.Uye_Id == id).SingleOrDefault();

                if (Convert.ToInt32(Session["Uye_Id"]) != kullanici.Uye_Id)
            {
                return HttpNotFound();
            }
            
            return View(kullanici);
        }

        public ActionResult login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Uye uye)
        {
            var login = db.Uyes.Where(u => u.Kullanici_Adi == uye.Kullanici_Adi).SingleOrDefault();
                if(login.Kullanici_Adi==uye.Kullanici_Adi &&login.Email==uye.Email&&login.Sifre==uye.Sifre)
            {
                Session["Uye_Id"] = login.Uye_Id;
                Session["Kullanici_Adi"] = login.Kullanici_Adi;
                Session["Yetki_id"] = login.Yetki_Id;
                return RedirectToAction("Hakkimizda", "Home");
            }
                else
            {
                ViewBag.Uyari = "Kullanici Adi,Mail yada şifrenizi kontrol ediniz !";
                return View("Login");
            }

        }

        public ActionResult Logout()
        {
            
            Session["Uye_Id"] = null;
            Session.Abandon();
            return RedirectToAction("Index", "Home");

        }


        public ActionResult Yeni_Kullanici()
        {
            return View();
        }

       [HttpPost]
        public ActionResult Yeni_Kullanici(Uye uye ,HttpPostedFileBase Foto)
        {   
            if (ModelState.IsValid)
            {
                if (Foto != null)
                {
                    WebImage img = new WebImage(Foto.InputStream);
                    FileInfo Fotoinfo = new FileInfo(Foto.FileName);
                    string newfoto = Guid.NewGuid().ToString() + Fotoinfo.Extension;
                    img.Resize(150, 150);
                    img.Save("~/Uploads/KullaniciFoto/" + newfoto);
                    uye.Foto = "/Uploads/KullaniciFoto/" + newfoto;
                    uye.Yetki_Id = 2;
                    db.Uyes.Add(uye);
                    db.SaveChanges();
                    Session["Uye_Id"] = uye.Uye_Id;
                    Session["Kullanici_Adi"] = uye.Kullanici_Adi;
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    ModelState.AddModelError("Fotograf ", "Fotograf Seciniz");
                    
                }
            }
            return View(uye);
        }

        public ActionResult Kullanici_Edit(int id )
        {
            var kullanici = db.Uyes.Where(u => u.Uye_Id == id).SingleOrDefault();
            if (Convert.ToInt32(Session["Uye_Id"])!=kullanici.Uye_Id)
            {
                return HttpNotFound();
            }
            return View(kullanici);
        }

        [HttpPost]
        public ActionResult Kullanici_Edit(Uye kullanici,int id,HttpPostedFileBase Foto)
        {
            if (ModelState.IsValid)
            {
                var secilen_kullanici = db.Uyes.Where(u => u.Uye_Id == id).SingleOrDefault();
                if (Foto != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(secilen_kullanici.Foto)))
                    {
                        System.IO.File.Delete(Server.MapPath(secilen_kullanici.Foto));
                    }
                    WebImage img = new WebImage(Foto.InputStream);
                    FileInfo Fotoinfo = new FileInfo(Foto.FileName);
                    string newfoto = Guid.NewGuid().ToString() + Fotoinfo.Extension;
                    img.Resize(150, 150);
                    img.Save("~/Uploads/KullaniciFoto/" + newfoto);
                    secilen_kullanici.Foto = "/Uploads/KullaniciFoto/" + newfoto;
                }
                    secilen_kullanici.AdSoyad = kullanici.AdSoyad;
                    secilen_kullanici.Email = kullanici.Email;
                    secilen_kullanici.Kullanici_Adi = kullanici.Kullanici_Adi;
                    secilen_kullanici.Sifre = kullanici.Sifre;
                    db.SaveChanges();
                    Session["Kullanici_Adi"] = kullanici.Kullanici_Adi;
                    return RedirectToAction("Index","Home",new { id = secilen_kullanici.Uye_Id });
                

            }

            return View();
        }
    }
}