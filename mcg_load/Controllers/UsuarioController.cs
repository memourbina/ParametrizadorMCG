using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using mcg_load.Code.Helpers;
using mcg_load.Models;
using Microsoft.AspNet.Identity;

namespace mcg_load.Controllers
{
    public class UsuarioController : BaseController
    {
        // GET: Usuario
        public ActionResult Index()
        {
            ViewBag.ShowBackButton = true;

            var escParametrosList = UsuarioHelper.GetAspNetUsers();

            List<RegisterViewModel> registerViewModels = new List<RegisterViewModel>();
            foreach (var esusuarios in escParametrosList)
            {
                RegisterViewModel model = new RegisterViewModel()
                {
                    UserName = esusuarios.UserName
                };
                String tempo="";
                foreach (var rol in esusuarios.AspNetRoles)
                {
                    tempo = rol.Id;
                }
                model.UserRoles = tempo;
                registerViewModels.Add(model);
            }

            return View(registerViewModels);
        }

        public ActionResult UsuarioDetailsPage(string Id)
        {
            ViewBag.ShowBackButton = true;
            var escParametrosList = UsuarioHelper.GetAspNetUsers(Id);

            List<RegisterViewModel> registerViewModels = new List<RegisterViewModel>();
            foreach (var esusuarios in escParametrosList)
            {
                RegisterViewModel model = new RegisterViewModel()
                {
                    UserName = esusuarios.UserName
                };
                String tempo = "";
                foreach (var rol in esusuarios.AspNetRoles)
                {
                    tempo = rol.Id;
                }
                model.UserRoles = tempo;
                registerViewModels.Add(model);
            }

            return View(registerViewModels);
        }

        public ActionResult UsuarioPartial()
        {
            var escParametrosList = UsuarioHelper.GetAspNetUsers();

            List<RegisterViewModel> registerViewModels = new List<RegisterViewModel>();
            foreach (var esusuarios in escParametrosList)
            {
                RegisterViewModel model = new RegisterViewModel()
                {
                    UserName = esusuarios.UserName
                };
                String tempo = "";
                foreach (var rol in esusuarios.AspNetRoles)
                {
                    tempo = rol.Id;
                }
                model.UserRoles = tempo;
                registerViewModels.Add(model);
            }

            return PartialView("UsuarioPartial", registerViewModels);
        }

        [ValidateAntiForgeryToken]
        public ActionResult UsuarioCustomActionPartial(string customAction)
        {
            if (customAction == "delete")
                SafeExecute(() => PerformDelete());
            return UsuarioPartial();
        }

        [ValidateAntiForgeryToken]
        public ActionResult UsuarioAddNewPartial([ModelBinder(typeof(DevExpress.Web.Mvc.DevExpressEditorsBinder))] Esc_Horas esHoras)
        {
            //esHoras.fecha = DateTime.Now;
            esHoras.UserId = User.Identity.GetUserId();
            //esHoras.activo = true;

            return UpdateModelWithDataValidation(esHoras, HorasHelper.AddNewRecord);
        }

        [ValidateAntiForgeryToken]
        public ActionResult UsuarioUpdatePartial([ModelBinder(typeof(DevExpress.Web.Mvc.DevExpressEditorsBinder))] Esc_Horas esHoras)
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
            return UsuarioPartial();
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