using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mcg_load.Code;
using mcg_load.Code.Helpers;
using Microsoft.AspNet.Identity;

namespace mcg_load.Controllers
{
    public class CavidadesController : BaseController
    {
        private mcg_saturacionEntities db = new mcg_saturacionEntities();

        // GET: FileUpload
        public ActionResult Index()
        {
            ViewBag.ShowBackButton = true;
            int id_escenario = Session["id_escenario"] != null ? (int)Session["id_escenario"] : -1;
            int id_articulo = Session["id_articulo"] != null ? (int)Session["id_articulo"] : -1;

            var escCavidades = CavidadesHelper.GetEscCavidades(id_articulo, id_escenario);
            return View(escCavidades.ToList());
        }

        public ActionResult CavidadesDetailsPage(long id)
        {
            ViewBag.ShowBackButton = true;
            return View(CavidadesHelper.FindRecord(id));
        }

        public ActionResult CavidadesPartial()
        {
            int id_escenario = Session["id_escenario"] != null ? (int)Session["id_escenario"] : -1;
            int id_articulo = Session["id_articulo"] != null ? (int)Session["id_articulo"] : -1;

            var escCavidades = CavidadesHelper.GetEscCavidades(id_articulo, id_escenario);

            return PartialView("CavidadesPartial", escCavidades);
        }

        [ValidateAntiForgeryToken]
        public ActionResult CavidadesCustomActionPartial(string customAction)
        {
            if (customAction == "delete")
                SafeExecute(() => PerformDelete());
            return CavidadesPartial();
        }

        [ValidateAntiForgeryToken]
        public ActionResult CavidadesAddNewPartial([ModelBinder(typeof(DevExpress.Web.Mvc.DevExpressEditorsBinder))] Esc_Cavidades escCavidades)
        {
            //escCavidades.fecha = DateTime.Now;
            escCavidades.UserId = User.Identity.GetUserId();
            escCavidades.eliminado = false;
            //escCavidades.activo = true;

            return UpdateModelWithDataValidation(escCavidades, CavidadesHelper.AddNewRecord);
        }

        [ValidateAntiForgeryToken]
        public ActionResult CavidadesUpdatePartial([ModelBinder(typeof(DevExpress.Web.Mvc.DevExpressEditorsBinder))] Esc_Cavidades escCavidades)
        {
            escCavidades.UserId_modifico = User.Identity.GetUserId();
            if (!ModelState.IsValid)
            {
                return UpdateModelWithDataValidation(escCavidades, CavidadesHelper.AddNewRecord);
            }

            return UpdateModelWithDataValidation(escCavidades, CavidadesHelper.UpdateRecord);
        }

        private ActionResult UpdateModelWithDataValidation(Esc_Cavidades escCavidades,
            Action<Esc_Cavidades> updateMethod)
        {
            if (ModelState.IsValid)
                SafeExecute(() => updateMethod(escCavidades));
            else
                ViewBag.GeneralError = "Please, correct all errors.";
            return CavidadesPartial();
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