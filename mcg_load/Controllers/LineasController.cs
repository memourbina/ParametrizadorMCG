using mcg_load.Code.Helpers;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.Mvc;

namespace mcg_load.Controllers
{
    public class LineasController : BaseController
    {
        private static mcg_saturacionEntities db = new mcg_saturacionEntities();
        // GET: Lineas
        public ActionResult Index()
        {
            ViewBag.ShowBackButton = true;
            int id_escenario = Session["id_escenario"] != null ? (int)Session["id_escenario"] : -1;
            
            var escLineas = LineasHelper.GetEscLineas();
            return View(escLineas.ToList());
        }

        public ActionResult LineasPartial()
        {
            int id_escenario = Session["id_escenario"] != null ? (int)Session["id_escenario"] : -1;
            int id_articulo = Session["id_articulo"] != null ? (int)Session["id_articulo"] : -1;
            return PartialView("LineasPartial", LineasHelper.GetEscLineas());  //GetEscVelocidades(id_articulo, id_escenario));
        }

        public ActionResult LineasDetailsPage(String id)
        {
            ViewBag.ShowBackButton = true;
            return View(LineasHelper.FindRecord(id));
        }

        [ValidateAntiForgeryToken]
        public ActionResult LineasCustomActionPartial(string customAction)
        {
            String strLinea  = "";
            //todo: validar si editar trae varios registro
            string strTempo = !string.IsNullOrEmpty(Request.Params["SelectedRows"])
                ? Convert.ToString(Request.Params["SelectedRows"])
                : "";

            var listTempo = strTempo.Split(',');

            if (listTempo.Length > 1)
            {
                strLinea = listTempo[0];
                Session["Linea"] = strLinea;
            }
            else
            {
                if (!string.IsNullOrEmpty(listTempo[0]))
                {
                    strLinea = listTempo[0];
                    Session["Linea"] = strLinea;
                }
            }

            int id_escenario = Session["id_escenario"] != null ? (int)Session["id_escenario"] : -1;

            switch (customAction)
            {
                case "delete":
                    SafeExecute(() => PerformDelete());
                    break;

                case "Horas":
                    return RedirectToAction("Index", "Horas", id_escenario);
                    //break;
                case "OEE":
                    return RedirectToAction("Index", "OEE", id_escenario);
                    //break;
                case "OEEDesglose":
                    return RedirectToAction("Index", "OEEDesglose", id_escenario);
                    //break;
            }

            if (customAction == "delete")
                SafeExecute(() => PerformDelete());
            return LineasPartial();
        }

        [ValidateAntiForgeryToken]
        public ActionResult LineasAddNewPartial([ModelBinder(typeof(DevExpress.Web.Mvc.DevExpressEditorsBinder))] Esc_Lineas escLineas)
        {
            //escVelocidad.fecha = DateTime.Now;
            //escLineas.UserId = User.Identity.GetUserId();
            //escLineas.eliminado = false;
            //escVelocidad.activo = true;

            return UpdateModelWithDataValidation(escLineas, LineasHelper.AddNewRecord);
        }

        [ValidateAntiForgeryToken]
        public ActionResult LineasUpdatePartial([ModelBinder(typeof(DevExpress.Web.Mvc.DevExpressEditorsBinder))] Esc_Lineas escLineas)
        {
            //escLineas.UserId_modifico = User.Identity.GetUserId();
            if (!ModelState.IsValid)
            {
                return UpdateModelWithDataValidation(escLineas, LineasHelper.AddNewRecord);
            }

            return UpdateModelWithDataValidation(escLineas, LineasHelper.CopyRecord);
        }

        private ActionResult UpdateModelWithDataValidation(Esc_Lineas escLineas,
            Action<Esc_Lineas> updateMethod)
        {
            if (ModelState.IsValid)
                SafeExecute(() => updateMethod(escLineas));
            else
                ViewBag.GeneralError = "Please, correct all errors.";
            return LineasPartial();
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