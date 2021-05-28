using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mcg_load.Code.Helpers;
using Microsoft.AspNet.Identity;

namespace mcg_load.Controllers
{
    public class HorasController : BaseController
    {
        // GET: Horas
        public ActionResult Index()
        {
            ViewBag.ShowBackButton = true;
            int id_escenario = Session["id_escenario"] != null ? (int)Session["id_escenario"] : -1;
            string Linea = (string) Session["Linea"] ;

            var escParametrosList = HorasHelper.GetEscHoras(id_escenario, Linea);
            return View(escParametrosList.ToList());
        }

        public ActionResult HorasDetailsPage(long id)
        {
            ViewBag.ShowBackButton = true;
            return View(HorasHelper.FindRecord(id));
        }

        public ActionResult HorasPartial()
        {
            int id_escenario = Session["id_escenario"] != null ? (int)Session["id_escenario"] : -1;
            string Linea = (string)Session["Linea"];

            var escParametrosList = HorasHelper.GetEscHoras(id_escenario, Linea);
            return PartialView("HorasPartial", escParametrosList);
        }

        [ValidateAntiForgeryToken]
        public ActionResult HorasCustomActionPartial(string customAction)
        {
            if (customAction == "delete")
                SafeExecute(() => PerformDelete());
            return HorasPartial();
        }

        [ValidateAntiForgeryToken]
        public ActionResult HorasAddNewPartial([ModelBinder(typeof(DevExpress.Web.Mvc.DevExpressEditorsBinder))] Esc_Horas esHoras)
        {
            //esHoras.fecha = DateTime.Now;
            esHoras.UserId = User.Identity.GetUserId();
            //esHoras.activo = true;

            return UpdateModelWithDataValidation(esHoras, HorasHelper.AddNewRecord);
        }

        [ValidateAntiForgeryToken]
        public ActionResult ParametrosUpdatePartial([ModelBinder(typeof(DevExpress.Web.Mvc.DevExpressEditorsBinder))] Esc_Horas esHoras)
        {
            if (!ModelState.IsValid)
            {
                return UpdateModelWithDataValidation(esHoras, HorasHelper.AddNewRecord);
            }

            return UpdateModelWithDataValidation(esHoras, HorasHelper.UpdateRecord);
        }

        private ActionResult UpdateModelWithDataValidation(Esc_Horas esHoras,
            Action<Esc_Horas> updateMethod)
        {
            if (ModelState.IsValid)
                SafeExecute(() => updateMethod(esHoras));
            else
                ViewBag.GeneralError = "Please, correct all errors.";
            return HorasPartial();
        }

        private void PerformDelete()
        {
            if (!string.IsNullOrEmpty(Request.Params["SelectedRows"]))
                HorasHelper.DeleteRecords(Request.Params["SelectedRows"]);
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