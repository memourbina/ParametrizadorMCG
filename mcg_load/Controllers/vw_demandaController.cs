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
    public class vw_demandaController : Controller
    {
        private mcg_saturacionEntities db = new mcg_saturacionEntities();

        // GET: vw_demanda
        public ActionResult Index()
        {
            return View(db.vw_demanda.ToList());
        }



        public List<vw_demanda> GetCustomers()
        {
            return db.vw_demanda.ToList();
        }

        public ActionResult GridViewDetailsPage(long id)
        {
            ViewBag.ShowBackButton = true;
            return View(db.vw_demanda.FirstOrDefault(b => Math.Abs(b.id_articulo - id) < 0));
        }


        // GET: vw_demanda/Details/5
        public ActionResult Details(double? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vw_demanda vw_demanda = db.vw_demanda.Find(id);
            if (vw_demanda == null)
            {
                return HttpNotFound();
            }
            return View(vw_demanda);
        }

        // GET: vw_demanda/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: vw_demanda/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_articulo,id_escenario,Planta,Org__ID,Escenario,Probabilidad,Línea_Moldeo,Cliente,Customer_Name,Industria,Tipo_Pieza,Description,No__Oracle,Customer_PN,Unit_Weight,Mercado,Tipo_de_Hierro,Grado_de_Hierro,OEM,Plataforma,Sistema,Cavs____Soplo,Soplos___Hora,Cor___pza,Maquina,Cantidad,Cantidad_base,Interno,Externo,Combinado,Interno_base,Externo_base,Combinado_base,Valor,Valor_base")] vw_demanda vw_demanda)
        {
            if (ModelState.IsValid)
            {
                db.vw_demanda.Add(vw_demanda);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vw_demanda);
        }

        // GET: vw_demanda/Edit/5
        public ActionResult Edit(double? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vw_demanda vw_demanda = db.vw_demanda.Find(id);
            if (vw_demanda == null)
            {
                return HttpNotFound();
            }
            return View(vw_demanda);
        }

        // POST: vw_demanda/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_articulo,id_escenario,Planta,Org__ID,Escenario,Probabilidad,Línea_Moldeo,Cliente,Customer_Name,Industria,Tipo_Pieza,Description,No__Oracle,Customer_PN,Unit_Weight,Mercado,Tipo_de_Hierro,Grado_de_Hierro,OEM,Plataforma,Sistema,Cavs____Soplo,Soplos___Hora,Cor___pza,Maquina,Cantidad,Cantidad_base,Interno,Externo,Combinado,Interno_base,Externo_base,Combinado_base,Valor,Valor_base")] vw_demanda vw_demanda)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vw_demanda).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vw_demanda);
        }

        // GET: vw_demanda/Delete/5
        public ActionResult Delete(double? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vw_demanda vw_demanda = db.vw_demanda.Find(id);
            if (vw_demanda == null)
            {
                return HttpNotFound();
            }
            return View(vw_demanda);
        }

        // POST: vw_demanda/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(double id)
        {
            vw_demanda vw_demanda = db.vw_demanda.Find(id);
            db.vw_demanda.Remove(vw_demanda);
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
