using System;
using System.Linq;
using System.Web.Mvc;
using mcg_load.Code.Helpers;

namespace mcg_load.Controllers
{
    public class DemandaController : BaseController
    {
        //private mcg_saturacionEntities db = new mcg_saturacionEntities();

        // GET: FileUpload
        public ActionResult Index()
        {
            ViewBag.ShowBackButton = true;
            int id_escenario = Session["id_escenario"] != null ? (int)Session["id_escenario"] : -1;
            int id_articulo = Session["id_articulo"] != null ? (int)Session["id_articulo"] : -1;

            var vwDemanda01s = DemandaHelper.GetEscDemanda01List(id_articulo, id_escenario);
            return View(vwDemanda01s.ToList());
        }

        public ActionResult DemandaDetailsPage(long id)
        {
            ViewBag.ShowBackButton = true;
            return View(DemandaHelper.FindRecord(id));
        }

        public ActionResult DemandaPartial()
        {
            int id_escenario = Session["id_escenario"] != null ? (int)Session["id_escenario"] : -1;
            int id_articulo = Session["id_articulo"] != null ? (int)Session["id_articulo"] : -1;

            var escDemanda01 = DemandaHelper.GetEscDemanda01List(id_articulo, id_escenario);

            return PartialView("DemandaPartial", escDemanda01);
        }

        [ValidateAntiForgeryToken]
        public ActionResult DemandaCustomActionPartial(string customAction)
        {
            if (customAction == "delete")
                SafeExecute(() => PerformDelete());
            return DemandaPartial();
        }

        [ValidateAntiForgeryToken]
        public ActionResult DemandaAddNewPartial([ModelBinder(typeof(DevExpress.Web.Mvc.DevExpressEditorsBinder))] vw_demanda01 escDemanda01s)
        {
            //escDemanda01s.fecha = DateTime.Now;
            //escDemanda01s. .UserId = User.Identity.GetUserId();
            //escDemanda01s.eliminado = false;
            //escDemanda01s.activo = true;

            return UpdateModelWithDataValidation(escDemanda01s, DemandaHelper.AddNewRecord);
        }

        [ValidateAntiForgeryToken]
        public ActionResult DemandaUpdatePartial([ModelBinder(typeof(DevExpress.Web.Mvc.DevExpressEditorsBinder))] vw_demanda01 escDemanda01s)
        {
            if (!ModelState.IsValid)
            {
                return UpdateModelWithDataValidation(escDemanda01s, DemandaHelper.AddNewRecord);
            }

            return UpdateModelWithDataValidation(escDemanda01s, DemandaHelper.UpdateRecord);
        }

        private ActionResult UpdateModelWithDataValidation(vw_demanda01 escDemanda01s,
            Action<vw_demanda01> updateMethod)
        {
            if (ModelState.IsValid)
                SafeExecute(() => updateMethod(escDemanda01s));
            else
                ViewBag.GeneralError = "Please, correct all errors.";
            return DemandaPartial();
        }

        private void PerformDelete()
        {
            if (!string.IsNullOrEmpty(Request.Params["SelectedRows"]))
                DemandaHelper.DeleteRecords(Request.Params["SelectedRows"]);
        }

        public ActionResult ExternalEditFormDelete(int productID = -1)
        {
            return View("Index");
        }

        //public ActionResult DemandaMovingPartial()
        //{
        //    int id_escenario = Session["id_escenario"] != null ? (int)Session["id_escenario"] : -1;
        //    int id_articulo = Session["id_articulo"] != null ? (int)Session["id_articulo"] : -1;

        //    var escDemanda01 = DemandaHelper.GetEscDemanda01List(id_articulo, id_escenario);

        //    return PartialView("DemandaPartial", escDemanda01);
        //}

        //[HttpPost, ValidateInput(false)]
        //public ActionResult DemandaMovingPartial(DemandaMoving demandaMoving)
        //{
        //    if (!ModelState.IsValid)
        //        return View("DemandaMovingPartial", demandaMoving);

        //    if (demandaMoving.fechaDesde > demandaMoving.fechaHasta)
        //        return View("DemandaMovingPartial", demandaMoving);
                          

            
        //    return View("Index");
        //}
    }
}