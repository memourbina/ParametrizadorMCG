using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mcg_load.Code.Helpers;
using Microsoft.AspNet.Identity;

namespace mcg_load.Controllers
{
    public class RechazosController : BaseController
    {
        private mcg_saturacionEntities db = new mcg_saturacionEntities();

        // GET: FileUpload
        public ActionResult Index()
        {
            ViewBag.ShowBackButton = true;
            int id_escenario = Session["id_escenario"] != null ? (int)Session["id_escenario"] : -1;
            int id_articulo = Session["id_articulo"] != null ? (int)Session["id_articulo"] : -1;
            
            var escRechazos = RechazoHelper.GetEscRechazos(id_articulo, id_escenario);
            return View(escRechazos.ToList());
        }

        public ActionResult RechazosDetailsPage(long id)
        {
            ViewBag.ShowBackButton = true;
            return View(RechazoHelper.FindRecord(id));
        }

        public ActionResult RechazosPartial()
        {
            int id_escenario = Session["id_escenario"] != null ? (int)Session["id_escenario"] : -1;
            int id_articulo = Session["id_articulo"] != null ? (int)Session["id_articulo"] : -1;

            var escRechazos = RechazoHelper.GetEscRechazos(id_articulo, id_escenario);
            return PartialView("RechazosPartial", escRechazos);
        }

        [ValidateAntiForgeryToken]
        public ActionResult RechazosCustomActionPartial(string customAction)
        {
            if (customAction == "delete")
                SafeExecute(() => PerformDelete());
            return RechazosPartial();
        }

        [ValidateAntiForgeryToken]
        public ActionResult RechazosAddNewPartial([ModelBinder(typeof(DevExpress.Web.Mvc.DevExpressEditorsBinder))] Esc_Rechazo esRechazo)
        {
            //esRechazo.fecha = DateTime.Now;
            esRechazo.UserId = User.Identity.GetUserId();
            esRechazo.eliminado = false;
            //esRechazo.activo = true;

            return UpdateModelWithDataValidation(esRechazo, RechazoHelper.AddNewRecord);
        }

        [ValidateAntiForgeryToken]
        public ActionResult RechazosUpdatePartial([ModelBinder(typeof(DevExpress.Web.Mvc.DevExpressEditorsBinder))] Esc_Rechazo esRechazo)
        {
            esRechazo.UserId = User.Identity.GetUserId();
            esRechazo.UserId_modifico = User.Identity.GetUserId();
            if (!ModelState.IsValid)
            {
                return UpdateModelWithDataValidation(esRechazo, RechazoHelper.AddNewRecord);
            }

            return UpdateModelWithDataValidation(esRechazo, RechazoHelper.UpdateRecord);
        }

        private ActionResult UpdateModelWithDataValidation(Esc_Rechazo esRechazo,
            Action<Esc_Rechazo> updateMethod)
        {
            if (ModelState.IsValid)
                SafeExecute(() => updateMethod(esRechazo));
            else
                ViewBag.GeneralError = "Please, correct all errors.";
            return RechazosPartial();
        }

        private void PerformDelete()
        {
            if (!string.IsNullOrEmpty(Request.Params["SelectedRows"]))
                RechazoHelper.DeleteRecords(Request.Params["SelectedRows"]);
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