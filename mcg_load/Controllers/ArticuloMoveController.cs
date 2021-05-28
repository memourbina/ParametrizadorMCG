using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mcg_load.Code.Helpers;
using mcg_load.Models;

namespace mcg_load.Controllers
{
    public class ArticuloMoveController : BaseController
    {

        private List<CArticuloMov> Movimiento(List<Esc_Articulos> model)
        {

            Esc_Articulos escArticulos = model[0];

            List<CArticuloMov> cArticuloMovs = new List<CArticuloMov>();
            CArticuloMov articuloMov = new CArticuloMov()
            {
                id_articulo = escArticulos.id_articulo,
                id_escenario = escArticulos.id_escenario,
                Org__ID = escArticulos.Org__ID,
                Escenario = escArticulos.Escenario,
                Probabilidad = escArticulos.Probabilidad,
                Línea_Moldeo = escArticulos.Línea_Moldeo,
                Cliente = escArticulos.Cliente,
                Customer_Name = escArticulos.Customer_Name,
                Industria = escArticulos.Industria,
                Tipo_Pieza = escArticulos.Tipo_Pieza,
                Description = escArticulos.Description,
                No__Oracle = escArticulos.No__Oracle,
                Customer_PN = escArticulos.Customer_PN,
                Unit_Weight = escArticulos.Unit_Weight,
                Mercado = escArticulos.Mercado,
                Tipo_de_Hierro = escArticulos.Tipo_de_Hierro,
                Grado_de_Hierro = escArticulos.Grado_de_Hierro,
                OEM = escArticulos.OEM,
                Plataforma = escArticulos.Plataforma,
                Sistema = escArticulos.Sistema,
                Cavs____Soplo = escArticulos.Cavs____Soplo,
                Soplos___Hora = escArticulos.Soplos___Hora,
                Cor___pza = escArticulos.Cor___pza,
                Maquina = escArticulos.Maquina,
                id_maquina = escArticulos.id_maquina,
                Planta = escArticulos.Planta,
                Planta01 = escArticulos.Planta01,
                Linea = escArticulos.Linea,
                modificado = escArticulos.modificado,
                UserId_modifico = escArticulos.UserId_modifico,
                fecha_modificacion = escArticulos.fecha_modificacion,
                fecha_start = DateTime.Now,
                fecha_end = DateTime.Now,
                tipo_traslado = "1",
                Cantidad = 0,
            };
            cArticuloMovs.Add(articuloMov);

            return cArticuloMovs;
        }

        // GET: ArticuloMove
        public ActionResult Index(ArticuloKey kyArticuloKey)
        {
            ViewBag.ShowBackButton = true;
            List<Esc_Articulos> model = null;
            int id_Escenario = Session["id_escenario"] != null ? (int)Session["id_escenario"] : -1;

            if (kyArticuloKey == null)
            {
                model = ArticuloHelper.GetEscArticulosList(id_Escenario);
                return View(model);
            }

            model = ArticuloHelper.GetEscArticulosList(kyArticuloKey.id_escenario, kyArticuloKey.id_articulo);

            List<CArticuloMov> cArticuloMovs = Movimiento(model);

            return View(cArticuloMovs);
        }

        public ActionResult ArticuloMoveDetailsPage(ArticuloKey kyArticuloKey)
        {
            ViewBag.ShowBackButton = true;
            return View(ArticuloHelper.FindRecord(kyArticuloKey));
        }

        public ActionResult ArticuloMovePartial()
        {
            int id_escenario = Session["id_escenario"] != null ? (int)Session["id_escenario"] : -1;
            int id_articulo = Session["id_articulo"] != null ? (int)Session["id_articulo"] : -1;

            var model = ArticuloHelper.GetEscArticulosList(id_escenario, id_articulo);
            List<CArticuloMov> cArticuloMovs = Movimiento(model);

            return PartialView("ArticuloMovePartial", cArticuloMovs);
        }

        [ValidateAntiForgeryToken]
        public ActionResult ArticuloMoveCustomActionPartial(string customAction)
        {
            if (customAction == "delete")
                SafeExecute(() => PerformDelete());
            return ArticuloMovePartial();
        }

        [ValidateAntiForgeryToken]
        public ActionResult ArticuloMoveAddNewPartial([ModelBinder(typeof(DevExpress.Web.Mvc.DevExpressEditorsBinder))]
            CArticuloMov escArticulos)
        {
            //escCavidades.fecha = DateTime.Now;
            //escArticulos.UserId = User.Identity.GetUserId();
            //escArticulos.eliminado = false;
            //escCavidades.activo = true;

            return UpdateModelWithDataValidation(escArticulos, ArticuloHelper.AddNewRecordMov);
        }

        [ValidateAntiForgeryToken]
        public ActionResult ArticuloMoveUpdatePartial([ModelBinder(typeof(DevExpress.Web.Mvc.DevExpressEditorsBinder))] CArticuloMov articuloMov)
        {
            //Esc_Articulos.UserId_modifico = User.Identity.GetUserId();
            if (!ModelState.IsValid)
            {
                return UpdateModelWithDataValidation(articuloMov, ArticuloHelper.AddNewRecordMov);
            }

            return UpdateModelWithDataValidation(articuloMov, ArticuloHelper.MoveRecord);
        }

        private ActionResult UpdateModelWithDataValidation(CArticuloMov articuloMov, Action<CArticuloMov> updateMethod)
        {
            if (ModelState.IsValid)
                SafeExecute(() => updateMethod(articuloMov));
            else
                ViewBag.GeneralError = "Please, correct all errors.";
            return ArticuloMovePartial();
        }

        private void PerformDelete()
        {
            if (!string.IsNullOrEmpty(Request.Params["SelectedRows"]))
                CavidadesHelper.DeleteRecords(Request.Params["SelectedRows"]);
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
    }
}