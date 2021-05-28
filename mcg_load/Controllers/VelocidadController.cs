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
    public class VelocidadController : BaseController
    {
        private mcg_saturacionEntities db = new mcg_saturacionEntities();

        // GET: FileUpload
        public ActionResult Index()
        {
            ViewBag.ShowBackButton = true;
            int id_escenario = Session["id_escenario"]!= null ? (int)Session["id_escenario"]: -1;
            int id_articulo = Session["id_articulo"] != null ? (int)Session["id_articulo"] : -1;
            var escVelocidades = VelocidadHelper.GetEscVelocidades(id_articulo, id_escenario);
            return View(escVelocidades.ToList());
        }

        public ActionResult VelocidadDetailsPage(long id)
        {
            ViewBag.ShowBackButton = true;
            return View(VelocidadHelper.FindRecord(id));
        }

        public ActionResult VelocidadPartial()
        {
            int id_escenario = Session["id_escenario"] != null ? (int)Session["id_escenario"] : -1;
            int id_articulo = Session["id_articulo"] != null ? (int)Session["id_articulo"] : -1;
            return PartialView("VelocidadPartial", VelocidadHelper.GetEscVelocidades(id_articulo, id_escenario));
        }

        [ValidateAntiForgeryToken]
        public ActionResult VelocidadCustomActionPartial(string customAction)
        {
            if (customAction == "delete")
                SafeExecute(() => PerformDelete());
            return VelocidadPartial();
        }

        [ValidateAntiForgeryToken]
        public ActionResult VelocidadAddNewPartial([ModelBinder(typeof(DevExpress.Web.Mvc.DevExpressEditorsBinder))] Esc_Velocidad escVelocidad)
        {
            //escVelocidad.fecha = DateTime.Now;
            escVelocidad.UserId = User.Identity.GetUserId();
            escVelocidad.eliminado = false;
            //escVelocidad.activo = true;

            return UpdateModelWithDataValidation(escVelocidad, VelocidadHelper.AddNewRecord);
        }

        [ValidateAntiForgeryToken]
        public ActionResult VelocidadUpdatePartial([ModelBinder(typeof(DevExpress.Web.Mvc.DevExpressEditorsBinder))] Esc_Velocidad escVelocidad)
        {
            escVelocidad.UserId_modifico = User.Identity.GetUserId();
            if (!ModelState.IsValid)
            {
                return UpdateModelWithDataValidation(escVelocidad, VelocidadHelper.AddNewRecord);
            }

            return UpdateModelWithDataValidation(escVelocidad, VelocidadHelper.UpdateRecord);
        }

        private ActionResult UpdateModelWithDataValidation(Esc_Velocidad escVelocidad,
            Action<Esc_Velocidad> updateMethod)
        {
            if (ModelState.IsValid)
                SafeExecute(() => updateMethod(escVelocidad));
            else
                ViewBag.GeneralError = "Please, correct all errors.";
            return VelocidadPartial();
        }

        private void PerformDelete()
        {
            if (!string.IsNullOrEmpty(Request.Params["SelectedRows"]))
                VelocidadHelper.DeleteRecords(Request.Params["SelectedRows"]);
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