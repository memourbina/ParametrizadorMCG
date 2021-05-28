using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mcg_load.Code;

namespace mcg_load.Controllers
{
    public class SaturacionController : BaseController
    {
        private mcg_saturacionEntities db = new mcg_saturacionEntities();

        // GET: GridView
        public ActionResult Index()
        {
            return View(SaturacionHelper.GetSaturacions());
        }

        public ActionResult SaturacionDetailsPage(long id)
        {
            ViewBag.ShowBackButton = true;
            return View(db.vw_demanda.FirstOrDefault(i => i.id_articulo == id));
        }

        public ActionResult SaturacionPartial()
        {
            return PartialView("SaturacionPartial", SaturacionHelper.GetSaturacions());
        }

        [ValidateAntiForgeryToken]
        public ActionResult SaturacionCustomActionPartial(string customAction)
        {
            if (customAction == "delete")
                SafeExecute(() => PerformDelete());
            return SaturacionPartial();
        }

        [ValidateAntiForgeryToken]
        public ActionResult SaturacionAddNewPartial(vw_demanda vwDemanda)
        {
            return UpdateModelWithDataValidation(vwDemanda, SaturacionHelper.AddNewRecord);
        }

        [ValidateAntiForgeryToken]
        public ActionResult SaturacionUpdatePartial(vw_demanda vwDemanda)
        {
            return UpdateModelWithDataValidation(vwDemanda, SaturacionHelper.UpdateRecord);
        }

        private ActionResult UpdateModelWithDataValidation(vw_demanda vwDemanda, Action<vw_demanda> updateMethod)
        {
            //if (ModelState.IsValid)
            //    SafeExecute(() => updateMethod(vw_demanda));
            //else
            //    ViewBag.GeneralError = "Please, correct all errors.";
            return SaturacionPartial();
        }

        private void PerformDelete()
        {
            if (!string.IsNullOrEmpty(Request.Params["SelectedRows"]))
                GridViewHelper.DeleteRecords(Request.Params["SelectedRows"]);
        }
    }
}