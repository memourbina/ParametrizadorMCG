using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mcg_load;

namespace mcg_load.Controllers
{
    public class CAT_PlantasController : Controller
    {
        private mcg_saturacionEntities db = new mcg_saturacionEntities();

        // GET: CAT_Plantas
        public ActionResult Index()
        {
            return View(db.CAT_Plantas.ToList());
        }

        // GET: CAT_Plantas/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CAT_Plantas cAT_Plantas = db.CAT_Plantas.Find(id);
            if (cAT_Plantas == null)
            {
                return HttpNotFound();
            }
            return View(cAT_Plantas);
        }

        // GET: CAT_Plantas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CAT_Plantas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Planta,org_id,Nombre,nombre_planta,OI,Site,Site_largo")] CAT_Plantas cAT_Plantas)
        {
            if (ModelState.IsValid)
            {
                db.CAT_Plantas.Add(cAT_Plantas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cAT_Plantas);
        }

        // GET: CAT_Plantas/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CAT_Plantas cAT_Plantas = db.CAT_Plantas.Find(id);
            if (cAT_Plantas == null)
            {
                return HttpNotFound();
            }
            return View(cAT_Plantas);
        }

        // POST: CAT_Plantas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Planta,org_id,Nombre,nombre_planta,OI,Site,Site_largo")] CAT_Plantas cAT_Plantas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cAT_Plantas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cAT_Plantas);
        }

        // GET: CAT_Plantas/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CAT_Plantas cAT_Plantas = db.CAT_Plantas.Find(id);
            if (cAT_Plantas == null)
            {
                return HttpNotFound();
            }
            return View(cAT_Plantas);
        }

        // POST: CAT_Plantas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CAT_Plantas cAT_Plantas = db.CAT_Plantas.Find(id);
            db.CAT_Plantas.Remove(cAT_Plantas);
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
