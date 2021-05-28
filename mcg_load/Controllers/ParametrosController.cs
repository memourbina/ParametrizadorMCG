using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mcg_load.Code.Helpers;
using Microsoft.AspNet.Identity;

namespace mcg_load.Controllers
{
    public class ParametrosController : BaseController
    {
        // GET: Parametros
        public ActionResult Index()
        {
            ViewBag.ShowBackButton = true;
            int id_escenario = Session["id_escenario"] != null ? (int)Session["id_escenario"] : -1;
            int id_articulo = Session["id_articulo"] != null ? (int)Session["id_articulo"] : -1;

            var escParametrosList = ParametrosHelper.GetEscParametros(id_articulo, id_escenario);
            return View(escParametrosList.ToList());
        }

        public ActionResult ParametrosDetailsPage(long id)
        {
            ViewBag.ShowBackButton = true;
            return View(ParametrosHelper.FindRecord(id));
        }

        public ActionResult ParametrosPartial()
        {
            int id_escenario = Session["id_escenario"] != null ? (int)Session["id_escenario"] : -1;
            int id_articulo = Session["id_articulo"] != null ? (int)Session["id_articulo"] : -1;

            var escParametrosList = ParametrosHelper.GetEscParametros(id_articulo, id_escenario);
            return PartialView("ParametrosPartial", escParametrosList);
        }

        [ValidateAntiForgeryToken]
        public ActionResult ParametrosCustomActionPartial(string customAction)
        {
            if (customAction == "delete")
                SafeExecute(() => PerformDelete());
            return ParametrosPartial();
        }

        [ValidateAntiForgeryToken]
        public ActionResult ParametrosAddNewPartial([ModelBinder(typeof(DevExpress.Web.Mvc.DevExpressEditorsBinder))] Esc_Parametros escParametros)
        {
            //esRechazo.fecha = DateTime.Now;
            escParametros.UserId = User.Identity.GetUserId();
            //escParametros.eliminado = false;
            //esRechazo.activo = true;

            return UpdateModelWithDataValidation(escParametros, ParametrosHelper.AddNewRecord);
        }

        [ValidateAntiForgeryToken]
        public ActionResult ParametrosUpdatePartial([ModelBinder(typeof(DevExpress.Web.Mvc.DevExpressEditorsBinder))] Esc_Parametros escParametros)
        {
            escParametros.UserId_modifico = User.Identity.GetUserId();
            if (!ModelState.IsValid)
            {
                return UpdateModelWithDataValidation(escParametros, ParametrosHelper.AddNewRecord);
            }

            return UpdateModelWithDataValidation(escParametros, ParametrosHelper.UpdateRecord);
        }

        private ActionResult UpdateModelWithDataValidation(Esc_Parametros escParametros,
            Action<Esc_Parametros> updateMethod)
        {
            if (ModelState.IsValid)
                SafeExecute(() => updateMethod(escParametros));
            else
                ViewBag.GeneralError = "Please, correct all errors.";
            return ParametrosPartial();
        }

        private void PerformDelete()
        {
            if (!string.IsNullOrEmpty(Request.Params["SelectedRows"]))
                ParametrosHelper.DeleteRecords(Request.Params["SelectedRows"]);
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