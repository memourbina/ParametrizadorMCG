using System;
using System.Linq;
using System.Web.Mvc;
using mcg_load.Code;
using Microsoft.AspNet.Identity;

namespace mcg_load.Controllers
{
    public class EscenariosController : BaseController
    {
        private mcg_saturacionEntities db = new mcg_saturacionEntities();

        // GET: FileUpload
        public ActionResult Index(int id_tipo_escenario)
        {
            ViewBag.ShowBackButton = true;
            ViewBag.ShowBackButton = true;
            Session["id_tipo_escenario"] = id_tipo_escenario;

            var escEscenarios = EscenariosHelper.GetEscEscenarios(id_tipo_escenario);
            return View(escEscenarios.ToList());
        }

        public ActionResult EscenariosDetailsPage(long id)
        {
            ViewBag.ShowBackButton = true;
            return View(EscenariosHelper.FindRecord(id));
        }

        public ActionResult EscenariosPartial()
        {
            ViewBag.ShowBackButton = true;
            int id_tipo_escenario = Session["id_tipo_escenario"] != null ? (int)Session["id_tipo_escenario"] : -1;
            return PartialView("EscenariosPartial", EscenariosHelper.GetEscEscenarios(id_tipo_escenario));
        }

        [ValidateAntiForgeryToken]
        public ActionResult EscenariosCustomActionPartial(string customAction)
        {
            int id_Escenario = 0;
            //todo: validar si editar trae varios registro
            string strTempo = !string.IsNullOrEmpty(Request.Params["SelectedRows"])
                ? Convert.ToString(Request.Params["SelectedRows"])
                : "";

            var listTempo = strTempo.Split(',');

            if (listTempo.Length > 1)
            {
                id_Escenario = Convert.ToInt32(listTempo[0]);
                Session["id_escenario"] = id_Escenario;
            }
            else
            {
                if (!string.IsNullOrEmpty(listTempo[0]))
                {
                    id_Escenario = Convert.ToInt32(listTempo[0]);
                    Session["id_escenario"] = id_Escenario;
                }
            }
            
            switch (customAction)
            {
                case "delete":
                    SafeExecute(() => PerformDelete());
                    break;
                
                case "Lineas":                    
                    return RedirectToAction("Index", "Lineas", id_Escenario);
                    //break;
            }
            
            return EscenariosPartial();
        }

        [ValidateAntiForgeryToken]
        public ActionResult EscenariosAddNewPartial([ModelBinder(typeof(DevExpress.Web.Mvc.DevExpressEditorsBinder))]  Esc_Escenario escEscenario)
        {
            escEscenario.fecha = DateTime.Now;
            escEscenario.UserId = User.Identity.GetUserId();
            escEscenario.eliminado = false;
            escEscenario.activo = true;
            escEscenario.id_tipo_escenario = EscenariosHelper.GetEscTipoEscenarios("Escenario").id_tipo_escenario;

            return UpdateModelWithDataValidation(escEscenario, EscenariosHelper.AddNewRecord);
        }

        [ValidateAntiForgeryToken]
        public ActionResult EscenariosUpdatePartial([ModelBinder(typeof(DevExpress.Web.Mvc.DevExpressEditorsBinder))]  Esc_Escenario escEscenario)
        {
            if (!ModelState.IsValid)
            {
                return UpdateModelWithDataValidation(escEscenario, EscenariosHelper.AddNewRecord);
            }

            return UpdateModelWithDataValidation(escEscenario, EscenariosHelper.UpdateRecord);
        }

        private ActionResult UpdateModelWithDataValidation(Esc_Escenario escEscenario,
            Action<Esc_Escenario> updateMethod)
        {
            if (ModelState.IsValid)
                SafeExecute(() => updateMethod(escEscenario));
            else
                ViewBag.GeneralError = "Please, correct all errors.";
            return EscenariosPartial();
        }

        private void PerformDelete()
        {
            if (!string.IsNullOrEmpty(Request.Params["SelectedRows"]))
                EscenariosHelper.DeleteRecords(Request.Params["SelectedRows"]);
        }

        #region FormasCustom

        //public ActionResult ExternalEditFormEdit(int id_UpLoad = -1)
        //{
        //    if (id_UpLoad == -1)
        //    {
        //        var model = new UploadExcelViewModel() { FileAttach = null, Message = string.Empty, isValid = false };
        //        return View("EscenariosNewForm", model);
        //    }

        //    Esc_File_Upload escFileUpload = db.Esc_File_Upload.FirstOrDefault(t => t.id_UpLoad == id_UpLoad);

        //    return View("EscenariosEditingForm", escFileUpload);
        //}


        //[HttpPost, ValidateInput(false)]
        //public ActionResult ExternalEditFormEdit(Esc_Escenario escFileUpload)
        //{
        //    if (!ModelState.IsValid)
        //        return View("EscenariosEditingForm", escFileUpload);

        //    if (escFileUpload.id_UpLoad == -1)
        //        db.Esc_File_Upload.Add(escFileUpload);
        //    else
        //        db.Esc_File_Upload.AddOrUpdate(escFileUpload);

        //    db.SaveChanges();
        //    return View("Index");
        //}

        //[HttpPost]
        //public ActionResult EscenariosNewForm(UploadExcelViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    var validateExcel = new ValidateExcel(model, User.Identity.GetUserId());

        //    return View("EscenariosNewResultForm", validateExcel.TempoDataTable);
        //}

        #endregion FormasCustom

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