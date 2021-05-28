using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mcg_load.Code;
using mcg_load.Code.Helpers;
using mcg_load.Models;
using Microsoft.AspNet.Identity;

namespace mcg_load.Controllers
{
    public class ArticuloCopyController : BaseController
    {
        // GET: ArticuloCopy
        public ActionResult Index(ArticuloKey kyArticuloKey)
        {
            ViewBag.ShowBackButton = true;
            List<Esc_Articulos> model = null;
            int id_Escenario = Session["id_escenario"] != null ? (int) Session["id_escenario"] : -1;

            if (kyArticuloKey == null)
            {
                model = ArticuloHelper.GetEscArticulosList(id_Escenario);
                return View(model);
            }

            model = ArticuloHelper.GetEscArticulosList(kyArticuloKey.id_escenario, kyArticuloKey.id_articulo);
            //Esc_File_Upload escFileUpload = db.Esc_File_Upload.FirstOrDefault(t => t.id_UpLoad == id_UpLoad);

            return View(model);
        }

        public ActionResult ArticuloCopyDetailsPage(ArticuloKey kyArticuloKey)
        {
            ViewBag.ShowBackButton = true;
            return View(ArticuloHelper.FindRecord(kyArticuloKey));
        }

        public ActionResult ArticuloCopyPartial()
        {
            int id_escenario = Session["id_escenario"] != null ? (int) Session["id_escenario"] : -1;
            int id_articulo = Session["id_articulo"] != null ? (int) Session["id_articulo"] : -1;

            var escCavidades = ArticuloHelper.GetEscArticulosList(id_escenario, id_articulo);

            return PartialView("ArticuloCopyPartial", escCavidades);
        }

        [ValidateAntiForgeryToken]
        public ActionResult ArticuloCopyCustomActionPartial(string customAction)
        {
            if (customAction == "delete")
                SafeExecute(() => PerformDelete());
            return ArticuloCopyPartial();
        }

        [ValidateAntiForgeryToken]
        public ActionResult ArticuloCopyAddNewPartial([ModelBinder(typeof(DevExpress.Web.Mvc.DevExpressEditorsBinder))]
            Esc_Articulos escArticulos)
        {
            //escCavidades.fecha = DateTime.Now;
            //escArticulos.UserId = User.Identity.GetUserId();
            //escArticulos.eliminado = false;
            //escCavidades.activo = true;

            return UpdateModelWithDataValidation(escArticulos, ArticuloHelper.AddNewRecord);
        }

        [ValidateAntiForgeryToken]
        public ActionResult ArticuloCopyUpdatePartial([ModelBinder(typeof(DevExpress.Web.Mvc.DevExpressEditorsBinder))]
            Esc_Articulos escArticulos)
        {
            //Esc_Articulos.UserId_modifico = User.Identity.GetUserId();
            if (!ModelState.IsValid)
            {
                return UpdateModelWithDataValidation(escArticulos, ArticuloHelper.AddNewRecord);
            }

            return UpdateModelWithDataValidation(escArticulos, ArticuloHelper.CopyRecord);
        }

        private ActionResult UpdateModelWithDataValidation(Esc_Articulos escArticulos,
            Action<Esc_Articulos> updateMethod)
        {
            if (ModelState.IsValid)
                SafeExecute(() => updateMethod(escArticulos));
            else
                ViewBag.GeneralError = "Please, correct all errors.";
            return ArticuloCopyPartial();
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