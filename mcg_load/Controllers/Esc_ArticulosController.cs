using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DevExpress.Data.WcfLinq.Helpers;
using mcg_load;
using mcg_load.Models;

namespace mcg_load.Controllers
{
    public class Esc_ArticulosController : Controller
    {
        private mcg_saturacionEntities db = new mcg_saturacionEntities();

        // GET: Esc_Articulos
        public ActionResult Index()
        {
            var esc_Articulos = db.Esc_Articulos.Include(e => e.AspNetUsers).Include(e => e.CAT_Plantas).Include(e => e.Esc_Escenario).Include(e => e.Esc_Lineas).Take(10);
            return View(esc_Articulos.ToList());
        }

        // GET: Esc_Articulos/Details/5
        public ActionResult Details(ArticuloKey articuloKey)
        {
            if (articuloKey == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Esc_Articulos esc_Articulos = db.Esc_Articulos.FirstOrDefault(t => t.id_articulo==articuloKey.id_articulo && t.id_escenario == articuloKey.id_escenario);
            if (esc_Articulos == null)
            {
                return HttpNotFound();
            }
            return View(esc_Articulos);
        }

        // GET: Esc_Articulos/Create
        public ActionResult Create()
        {
            ViewBag.UserId_modifico = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.Planta01 = new SelectList(db.CAT_Plantas, "Planta", "Nombre");
            ViewBag.id_escenario = new SelectList(db.Esc_Escenario, "id_escenario", "UserId");
            ViewBag.Linea = new SelectList(db.Esc_Lineas, "Linea", "Planta");
            return View();
        }

        // POST: Esc_Articulos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_articulo,id_escenario,Org__ID,Escenario,Probabilidad,Línea_Moldeo,Cliente,Customer_Name,Industria,Tipo_Pieza,Description,No__Oracle,Customer_PN,Unit_Weight,Mercado,Tipo_de_Hierro,Grado_de_Hierro,OEM,Plataforma,Sistema,Cavs____Soplo,Soplos___Hora,Cor___pza,Maquina,Planta,Planta01,Linea,modificado,UserId_modifico,fecha_modificacion")] Esc_Articulos esc_Articulos)
        {
            if (ModelState.IsValid)
            {
                db.Esc_Articulos.Add(esc_Articulos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId_modifico = new SelectList(db.AspNetUsers, "Id", "Email", esc_Articulos.UserId_modifico);
            ViewBag.Planta01 = new SelectList(db.CAT_Plantas, "Planta", "Nombre", esc_Articulos.Planta01);
            ViewBag.id_escenario = new SelectList(db.Esc_Escenario, "id_escenario", "UserId", esc_Articulos.id_escenario);
            ViewBag.Linea = new SelectList(db.Esc_Lineas, "Linea", "Planta", esc_Articulos.Linea);
            return View(esc_Articulos);
        }

        // GET: Esc_Articulos/Edit/5
        public ActionResult Edit(ArticuloKey articuloKey)
        {
            if (articuloKey == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Esc_Articulos esc_Articulos = db.Esc_Articulos.FirstOrDefault(t => t.id_articulo == articuloKey.id_articulo && t.id_escenario == articuloKey.id_escenario); 
            if (esc_Articulos == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId_modifico = new SelectList(db.AspNetUsers, "Id", "Email", esc_Articulos.UserId_modifico);
            ViewBag.Planta01 = new SelectList(db.CAT_Plantas, "Planta", "Nombre", esc_Articulos.Planta01);
            ViewBag.id_escenario = new SelectList(db.Esc_Escenario, "id_escenario", "UserId", esc_Articulos.id_escenario);
            ViewBag.Linea = new SelectList(db.Esc_Lineas, "Linea", "Planta", esc_Articulos.Linea);
            return View(esc_Articulos);
        }

        // POST: Esc_Articulos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_articulo,id_escenario,Org__ID,Escenario,Probabilidad,Línea_Moldeo,Cliente,Customer_Name,Industria,Tipo_Pieza,Description,No__Oracle,Customer_PN,Unit_Weight,Mercado,Tipo_de_Hierro,Grado_de_Hierro,OEM,Plataforma,Sistema,Cavs____Soplo,Soplos___Hora,Cor___pza,Maquina,Planta,Planta01,Linea,modificado,UserId_modifico,fecha_modificacion")] Esc_Articulos esc_Articulos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(esc_Articulos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId_modifico = new SelectList(db.AspNetUsers, "Id", "Email", esc_Articulos.UserId_modifico);
            ViewBag.Planta01 = new SelectList(db.CAT_Plantas, "Planta", "Nombre", esc_Articulos.Planta01);
            ViewBag.id_escenario = new SelectList(db.Esc_Escenario, "id_escenario", "UserId", esc_Articulos.id_escenario);
            ViewBag.Linea = new SelectList(db.Esc_Lineas, "Linea", "Planta", esc_Articulos.Linea);
            return View(esc_Articulos);
        }

        // GET: Esc_Articulos/Delete/5
        public ActionResult Delete(double? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Esc_Articulos esc_Articulos = db.Esc_Articulos.Find(id);
            if (esc_Articulos == null)
            {
                return HttpNotFound();
            }
            return View(esc_Articulos);
        }

        // POST: Esc_Articulos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(double id)
        {
            Esc_Articulos esc_Articulos = db.Esc_Articulos.Find(id);
            db.Esc_Articulos.Remove(esc_Articulos);
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
