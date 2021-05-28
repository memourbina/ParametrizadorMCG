using mcg_load.Code.Helpers;
using mcg_load.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mcg_load.Controllers
{
    public class DemandaMovingController : Controller
    {
        // GET: DemandaMoving
        public ActionResult Index(ArticuloKey kyArticuloKey)
        {
            ViewBag.ShowBackButton = true;
            //List<Esc_Articulos> model = null;

            DemandaMoving demandaMoving = new DemandaMoving();

            if (kyArticuloKey != null)
            {
                ViewBag.ShowBackButton = true;
                int id_escenario = Session["id_escenario"] != null ? (int)Session["id_escenario"] : -1;
                int id_articulo = Session["id_articulo"] != null ? (int)Session["id_articulo"] : -1;


                demandaMoving.Start = new DateTime();
                demandaMoving.End = new DateTime();
                ////return View("DemandaMovingPage", demandaMoving);
                List<Esc_Articulos> articulos = ArticuloHelper.GetEscArticulosList(id_articulo, id_escenario);
                Esc_Articulos esc_Articulos = articulos != null ? articulos.FirstOrDefault() : null;
                if (esc_Articulos != null)
                {
                    demandaMoving.id_articulo = esc_Articulos.id_articulo;
                    demandaMoving.id_escenario = esc_Articulos.id_escenario;
                    demandaMoving.Planta = esc_Articulos.Planta;
                    demandaMoving.Linea = esc_Articulos.Linea;
                    demandaMoving.Probabilidad = esc_Articulos.Probabilidad;
                    demandaMoving.Cliente = esc_Articulos.Cliente;
                    demandaMoving.Customer_Name = esc_Articulos.Customer_Name;
                    demandaMoving.Industria = esc_Articulos.Industria;
                    demandaMoving.Tipo_Pieza = esc_Articulos.Tipo_Pieza;
                    demandaMoving.Description = esc_Articulos.Description;
                }
            }
            //demandaMoving.intValueOption = 1;
            return View(demandaMoving);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Index(DemandaMoving demandaMoving)
        {
            int id_escenario02 = Session["id_escenario"] != null ? (int)Session["id_escenario"] : -1;

            if (!ModelState.IsValid)
                return View("DemandaMovingPage", demandaMoving);


            string planta = (Request.Params["Option"] != null) ? Request.Params["Option"] : "-1";

        
            return RedirectToAction("Index", "Articulo", new { id_escenario = id_escenario02 });
        }

        public ActionResult PlantaPartial()
        {
            return PartialView(new DemandaMoving());
        }

        public ActionResult TipoMovimientoPartial()
        {
            return PartialView(new DemandaMoving());
        }
        

        public ActionResult LineaPartial()
        {
            string planta = (Request.Params["Planta"] != null) ? Request.Params["Planta"] : "-1";
            return PartialView(new DemandaMoving { Planta = planta });
        }
    }
}