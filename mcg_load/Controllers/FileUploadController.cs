using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mcg_load;
using mcg_load.Code;
using mcg_load.Models.upload;
using Microsoft.AspNet.Identity;

namespace mcg_load.Controllers
{
    public class FileUploadController : BaseController
    {
        private mcg_saturacionEntities db = new mcg_saturacionEntities();        

        // GET: FileUpload
        public ActionResult Index()
        {
            ViewBag.ShowBackButton = true;
            var esc_File_Upload = db.Esc_File_Upload.Include(e => e.AspNetUsers);
            return View(esc_File_Upload.ToList());
        }

        public ActionResult FileUploadDetailsPage(long id)
        {
            ViewBag.ShowBackButton = true;
            return View(db.Esc_File_Upload.FirstOrDefault(i => i.id_UpLoad == id));
        }

        public ActionResult FileUploadPartial()
        {
            return PartialView("FileUploadPartial", FileUploadHelper.GetFileUploads());
        }

        [ValidateAntiForgeryToken]
        public ActionResult FileUploadCustomActionPartial(string customAction)
        {
            switch (customAction)
            {
                case "delete":
                    SafeExecute(() => PerformDelete());
                    break;
                case "new":
                    //return View("FileUploadNewForm");
                    break;
            }
            
                
            return FileUploadPartial();
        }

        [ValidateAntiForgeryToken]
        public ActionResult FileUploadAddNewPartial([ModelBinder(typeof(DevExpress.Web.Mvc.DevExpressEditorsBinder))]
            Esc_File_Upload escFileUpload)
        {
            return UpdateModelWithDataValidation(escFileUpload, FileUploadHelper.AddNewRecord);
        }

        [ValidateAntiForgeryToken]
        public ActionResult FileUploadUpdatePartial([ModelBinder(typeof(DevExpress.Web.Mvc.DevExpressEditorsBinder))] Esc_File_Upload escFileUpload)
        {
            if (!ModelState.IsValid)
            {
                return UpdateModelWithDataValidation(escFileUpload, FileUploadHelper.AddNewRecord);
            }

            return UpdateModelWithDataValidation(escFileUpload, FileUploadHelper.UpdateRecord);
        }

        private ActionResult UpdateModelWithDataValidation(
            [ModelBinder(typeof(DevExpress.Web.Mvc.DevExpressEditorsBinder))]
            Esc_File_Upload escFileUpload, Action<Esc_File_Upload> updateMethod)
        {
            if (ModelState.IsValid)
                SafeExecute(() => updateMethod(escFileUpload));
            else
                ViewBag.GeneralError = "Please, correct all errors.";
            return FileUploadPartial();
        }

        private void PerformDelete()
        {
            if (!string.IsNullOrEmpty(Request.Params["SelectedRows"]))
                FileUploadHelper.DeleteRecords(Request.Params["SelectedRows"]);
        }

        public ActionResult FileUploadNewForm(int id_UpLoad = -1)
        {
            if (id_UpLoad == -1)
            {
                var model = new UploadExcelViewModel() { FileAttach = null, Message = string.Empty, isValid = false };
                return View("FileUploadNewForm", model);
            }

            Esc_File_Upload escFileUpload = db.Esc_File_Upload.FirstOrDefault(t => t.id_UpLoad == id_UpLoad);

            return View("FileUploadEditingForm", escFileUpload);
        }
        
        public ActionResult ExternalEditFormEdit(int id_UpLoad = -1)
        {
            ViewBag.ShowBackButton = true;
            if (id_UpLoad == -1)
            {
                var model = new UploadExcelViewModel() {FileAttach = null, Message = string.Empty, isValid = false};
                return View("FileUploadNewForm", model);
            }

            Esc_File_Upload escFileUpload = db.Esc_File_Upload.FirstOrDefault(t => t.id_UpLoad == id_UpLoad);

            return View("FileUploadEditingForm", escFileUpload);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult ExternalEditFormEdit(Esc_File_Upload escFileUpload)
        {
            if (!ModelState.IsValid)
                return View("FileUploadEditingForm", escFileUpload);

            if (escFileUpload.id_UpLoad == -1)
                db.Esc_File_Upload.Add(escFileUpload);
            else
                db.Esc_File_Upload.AddOrUpdate(escFileUpload);

            db.SaveChanges();
            return View("Index");
        }

        [HttpPost]
        public ActionResult FileUploadNewForm(UploadExcelViewModel model)
        {
            ViewBag.ShowBackButton = true;
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var validateExcel = new ValidateExcel(model, User.Identity.GetUserId());


            return View("FileUploadNewResultForm", validateExcel.UploadResult);
        }

        public ActionResult ExternalEditFormDelete(int id_UpLoad = -1)
        {
            //EditableProduct editProduct = NorthwindDataProvider.GetEditableProduct(productID);
            //if (editProduct == null)
            //{
            //    editProduct = new EditableProduct();
            //    editProduct.ProductID = -1;
            //}
            //return DemoView("ExternalEditForm", "EditingForm", editProduct);
            return PartialView("FileUploadPartial", FileUploadHelper.GetFileUploads());
        }

        #region wizard

        //// GET: FileUpload/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    Esc_File_Upload esc_File_Upload = db.Esc_File_Upload.Find(id);
        //    if (esc_File_Upload == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    return View(esc_File_Upload);
        //}

        //// GET: FileUpload/Create
        //public ActionResult Create()
        //{
        //    ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email");
        //    return View();
        //}

        //// POST: FileUpload/Create
        //// Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        //// más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(
        //    [Bind(Include = "id_UpLoad,ContentLength,ContentType,FileName,TempFileName,date_uplodad,UserId")]
        //    Esc_File_Upload esc_File_Upload)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Esc_File_Upload.Add(esc_File_Upload);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", esc_File_Upload.UserId);
        //    return View(esc_File_Upload);
        //}

        //// GET: FileUpload/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    Esc_File_Upload esc_File_Upload = db.Esc_File_Upload.Find(id);
        //    if (esc_File_Upload == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", esc_File_Upload.UserId);
        //    return View(esc_File_Upload);
        //}

        //// POST: FileUpload/Edit/5
        //// Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        //// más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(
        //    [Bind(Include = "id_UpLoad,ContentLength,ContentType,FileName,TempFileName,date_uplodad,UserId")]
        //    Esc_File_Upload esc_File_Upload)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(esc_File_Upload).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", esc_File_Upload.UserId);
        //    return View(esc_File_Upload);
        //}

        //// GET: FileUpload/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    Esc_File_Upload esc_File_Upload = db.Esc_File_Upload.Find(id);
        //    if (esc_File_Upload == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    return View(esc_File_Upload);
        //}

        //// POST: FileUpload/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Esc_File_Upload esc_File_Upload = db.Esc_File_Upload.Find(id);
        //    db.Esc_File_Upload.Remove(esc_File_Upload);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }

        //    base.Dispose(disposing);
        //}

        #endregion wizard
    }
}