using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Blog.Models;
using PagedList;
using PagedList.Mvc;


namespace Web_Blog.Controllers
{
    public class HomeController : Controller
    {
        blog_Db db = new blog_Db();
        // GET: Home
        public ActionResult Index(int Page=1)
        {
            var makale = db.Makales.OrderByDescending(m => m.Makale_Id).ToPagedList(Page,5);
            return View(makale);
        }

        public ActionResult Kategori_Makale(int id )
        {
            var makaleler = db.Makales.Where(m => m.Makale_Id ==id).ToList();


            return View(makaleler);
        }

        public ActionResult Makale_Detay(int id)
        {
            var makale = db.Makales.Where(m => m.Makale_Id == id).SingleOrDefault();        
            makale.Okunma = makale.Okunma + 1;         
            if (makale == null)
            {
                return HttpNotFound();
            }         
            return View(makale);
        }
        public ActionResult Hakkimizda()
        {
            return View();
        }
        public ActionResult Iletisim()
        {
            return View();
        }
        public ActionResult Kategori_Partial()
        {
            return View(db.Kategoris.ToList());
        }
        public JsonResult YorumYap(string yorum, int Makaleid)
        {
            var kullanici_id = Session["Uye_Id"];

            if (yorum != null)
            {
                db.Yorums.Add(new Yorum { Uye_Id = Convert.ToInt32(kullanici_id), Makale_Id = Makaleid, Icerik = yorum, Tarih = DateTime.Now });
                db.SaveChanges();
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public ActionResult YorumSil(int id)
        {
            var uyeid = Session["Uye_Id"];
            var yorum = db.Yorums.Where(y => y.Yorum_Id == id).SingleOrDefault();
            var makale = db.Makales.Where(m => m.Makale_Id == yorum.Makale_Id).SingleOrDefault();
            if (yorum.Uye_Id == Convert.ToInt32(uyeid))
            {
                db.Yorums.Remove(yorum);
                db.SaveChanges();
                return RedirectToAction("Makale_Detay", "Home", new { id = makale.Makale_Id });
            }
            else
            {
                return HttpNotFound();
            }

        }
        public ActionResult SonYorumlar()
        {

            return View(db.Yorums.OrderByDescending(y=>y.Yorum_Id).Take(5));

        }

    }
    }
