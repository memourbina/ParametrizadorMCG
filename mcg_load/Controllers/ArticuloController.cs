using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
//using DevExpress.Web.Demos.Mvc;
using DevExpress.Web.Mvc;
using mcg_load.Code.Helpers;
using mcg_load.Models;
using Microsoft.AspNet.Identity;

namespace mcg_load.Controllers
{
    public class ArticuloController : BaseController
    {
        public ActionResult Index(int id_escenario)
        {
            ViewBag.ShowBackButton = true;
            Session["id_escenario"] = id_escenario;
            var escArticulosList = ArticuloHelper.GetEscArticulosList(id_escenario);
            ViewBag.Message = id_escenario + "";
            return View(escArticulosList.ToList());
        }

        public ActionResult GetLineas(string Planta)
        {
            return GridViewExtension.GetComboBoxCallbackResult(p =>
            {
                p.ValueField = "Linea";
                p.TextField = "Linea";
                p.ValueType = typeof(string);
                if (String.IsNullOrEmpty(Planta))
                    p.BindList(ArticuloHelper.GetEscLineasList());
                else
                {
                    p.BindList(ArticuloHelper.GetEscLineasList(Planta));
                }
            });
        }

        public ActionResult ArticuloPartial()
        {
            int id_Escenario = Session["id_escenario"] != null ? (int)Session["id_escenario"] : -1;


            if (id_Escenario > 0)
            {
                return PartialView("ArticuloPartial", ArticuloHelper.GetEscArticulosList(id_Escenario));
            }
            else
            {
                return PartialView("ArticuloPartial", ArticuloHelper.GetEscArticulosList());
            }
        }

        [ValidateAntiForgeryToken]
        public ActionResult ArticuloCustomActionPartial(string customAction)
        {
            int id_Articulo = 0;
            //todo: validar si editar trae varios registro
            string strTempo = !string.IsNullOrEmpty(Request.Params["SelectedRows"])
                ? Convert.ToString(Request.Params["SelectedRows"])
                : "";

            var listTempo = strTempo.Split(',');

            if (listTempo.Length > 1)
            {
                id_Articulo = Convert.ToInt32(listTempo[0]);
                Session["id_articulo"] = id_Articulo;
            }
            else
            {
                if (!string.IsNullOrEmpty(listTempo[0]))
                {
                    id_Articulo = Convert.ToInt32(listTempo[0]);
                    Session["id_articulo"] = id_Articulo;
                }
            }

            int id_Escenario = Session["id_escenario"] != null ? (int)Session["id_escenario"] : -1;

            ArticuloKey kyArticuloKey = null;
            kyArticuloKey = new ArticuloKey(id_Articulo, id_Escenario);

            switch (customAction)
            {
                case "delete":
                    SafeExecute(() => PerformDelete());
                    break;
                case "demanda":
                    return RedirectToAction("Index", "Demanda");
                //break;
                case "velocidad":
                    return RedirectToAction("Index", "Velocidad");
                //break;
                case "cavidades":
                    return RedirectToAction("Index", "Cavidades");
                //break;
                case "rechazos":
                    return RedirectToAction("Index", "Rechazos");
                case "Horas":
                    return RedirectToAction("Index", "Horas");
                //break;
                case "HorasCorazon":
                    return RedirectToAction("Index", "HorasCorazon");
                //break;
                case "OEE":
                    return RedirectToAction("Index", "OEE");
                //break;
                case "OEECorazon":
                    return RedirectToAction("Index", "OEECorazon");
                //break;
                case "OEEDesglose":
                    return RedirectToAction("Index", "OEEDesglose");
                //break;
                case "parametros":
                    return RedirectToAction("Index", "Parametros");
                //break;
                case "Copy":                    
                    return RedirectToAction("Index", "ArticuloCopy", kyArticuloKey);
                //break;
                case "Moving":            
                    return RedirectToAction("Index", "ArticuloMove", kyArticuloKey);
                    //return DemandaMovingPartial();
                    
            }

            return ArticuloPartial();
        }

        [ValidateAntiForgeryToken]
        public ActionResult ArticuloAddNewPartial([ModelBinder(typeof(DevExpress.Web.Mvc.DevExpressEditorsBinder))]
            Esc_Articulos escArticulo)
        {
            escArticulo.fecha_modificacion = DateTime.Now;
            escArticulo.UserId_modifico = User.Identity.GetUserId();
            escArticulo.modificado = true;

            return UpdateModelWithDataValidation(escArticulo, ArticuloHelper.AddNewRecord);
        }

        [ValidateAntiForgeryToken]
        public ActionResult ArticuloUpdatePartial([ModelBinder(typeof(DevExpress.Web.Mvc.DevExpressEditorsBinder))]
            Esc_Articulos escArticulo)
        {
            if (!ModelState.IsValid)
            {
                return UpdateModelWithDataValidation(escArticulo, ArticuloHelper.AddNewRecord);
            }

            return UpdateModelWithDataValidation(escArticulo, ArticuloHelper.UpdateRecord);
        }

        private ActionResult UpdateModelWithDataValidation(Esc_Articulos escArticulo,
            Action<Esc_Articulos> updateMethod)
        {
            if (ModelState.IsValid)
            {
                escArticulo.UserId_modifico = User.Identity.GetUserId();
                switch (updateMethod.Method.Name)
                {
                    case "UpdateRecord":
                        if (!string.IsNullOrEmpty(Request.Params["SelectedRows"]))
                        {
                            ArticuloHelper.UpdateRecords(Request.Params["SelectedRows"], escArticulo);
                        }
                        else
                        {
                            ArticuloHelper.UpdateRecord(escArticulo);
                        }

                        break;

                    default:
                        SafeExecute(() => updateMethod(escArticulo));
                        break;
                }
            }
            else
                ViewBag.GeneralError = "Please, correct all errors.";

            return ArticuloPartial();
        }

        private void PerformDelete()
        {
            if (!string.IsNullOrEmpty(Request.Params["SelectedRows"]))
                ArticuloHelper.DeleteRecords(Request.Params["SelectedRows"]);
        }


        public ActionResult ExternalEditFormDelete(int productID = -1)
        {
            //EditableProduct editProduct = NorthwindDataProvider.GetEditableProduct(productID);
            //if (editProduct == null)
            //{
            //    editProduct = new EditableProduct();
            //    editProduct.ProductID = -1;
            //}
            //return DemoView("ExternalEditForm", "EditingForm", editProduct);
            return View("Index");
        }

        public ActionResult DemandaMovingPartial()
        {
            ViewBag.ShowBackButton = true;
            int id_escenario = Session["id_escenario"] != null ? (int)Session["id_escenario"] : -1;
            int id_articulo = Session["id_articulo"] != null ? (int)Session["id_articulo"] : -1;

            DemandaMoving demandaMoving = new DemandaMoving();
            demandaMoving.Start = new DateTime();
            demandaMoving.End = new DateTime();
            ////return View("DemandaMovingPage", demandaMoving);
            List<Esc_Articulos> articulos = ArticuloHelper.GetEscArticulosList(id_articulo, id_escenario);
            Esc_Articulos esc_Articulos = articulos != null ? articulos.FirstOrDefault(): null;
            if (esc_Articulos != null )
            {
                demandaMoving.id_articulo = esc_Articulos.id_articulo;
                demandaMoving.id_escenario = esc_Articulos.id_escenario;
                demandaMoving.Planta = esc_Articulos.Planta01;
                demandaMoving.Linea = esc_Articulos.Linea;
                demandaMoving.Probabilidad = esc_Articulos.Probabilidad;
                demandaMoving.Cliente = esc_Articulos.Cliente;
                demandaMoving.Customer_Name = esc_Articulos.Customer_Name;
                demandaMoving.Industria = esc_Articulos.Industria;
                demandaMoving.Tipo_Pieza = esc_Articulos.Tipo_Pieza;
                demandaMoving.Description = esc_Articulos.Description;
            }
            //var escDemanda01 = DemandaHelper.GetEscDemanda01List(id_articulo, id_escenario);

            return PartialView("DemandaMovingPage", demandaMoving);
            //return View("DemandaMovingPage", demandaMoving);
        }

        public ActionResult ExternalEditFormEdit(int id_UpLoad = -1)
        {
            int id_articulo = Session["id_articulo"] != null ? (int)Session["id_articulo"] : -1;
            
            //todo: validar si editar trae varios registro
            string strTempo = !string.IsNullOrEmpty(Request.Params["SelectedRows"])
                ? Convert.ToString(Request.Params["SelectedRows"])
                : "";

            var listTempo = strTempo.Split(',');

            if (listTempo.Length > 1)
            {
                id_articulo = Convert.ToInt32(listTempo[0]);
                Session["id_articulo"] = id_articulo;
            }
            else
            {
                if (!string.IsNullOrEmpty(listTempo[0]))
                {
                    id_articulo = Convert.ToInt32(listTempo[0]);
                    Session["id_articulo"] = id_articulo;
                }
            }


            ViewBag.ShowBackButton = true;
            int id_escenario = Session["id_escenario"] != null ? (int)Session["id_escenario"] : -1;
            
            DemandaMoving demandaMoving = new DemandaMoving();
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

            return View("DemandaMovingPage", demandaMoving);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult DemandaMovingPartial(DemandaMoving demandaMoving)
        {
            if (!ModelState.IsValid)
                return View("DemandaMovingPage", demandaMoving);

            if (demandaMoving.Start > demandaMoving.Start)
                return View("DemandaMovingPage", demandaMoving);

            return View("Index");
        }

        public ActionResult PlantaPartial()
        {
            return PartialView(new DemandaMoving());
        }

        public ActionResult LineaPartial()
        {
            string planta = (Request.Params["Planta"] != null) ? Request.Params["Planta"] : "-1";
            return PartialView(new DemandaMoving { Planta = planta });
        }



        public ActionResult Planta01Partial()
        {
            return PartialView("_Planta01Partial");
        }

        public ActionResult CallbackPanelPartial()
        {
            string planta = (Request.Params["Planta"] != null) ? Request.Params["Planta"] : "-1";
            return PartialView("_CallbackPanelPartial", new DemandaMoving { Planta = planta });
        }
    }
}