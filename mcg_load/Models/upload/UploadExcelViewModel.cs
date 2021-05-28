using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using mcg_load.Code.Helpers;

namespace mcg_load.Models.upload
{
    public class UploadExcelViewModel
    {
        #region Properties  

        /// <summary>  
        /// Gets or sets Image file property.  
        /// </summary>
        /// 
        [Required]
        [Display(Name = "Archivos permitidos .xlsx | .xls")]
        [AllowExtensions(Extensions = "xlsx,xls", ErrorMessage = "Por favor selecciones únicamente los archivos permitods .xlsx | .xls")]
        public HttpPostedFileBase FileAttach { get; set; }

        /// <summary>  
        /// Gets or sets message property.  
        /// </summary>
        [Required]
        [Display(Name = "Nombre de la Carga")]
        public string Nombre{ get; set; }


        /// <summary>  
        /// Gets or sets message property.  
        /// </summary>  
        [Required]
        [Display(Name = "Descripción de la Carga")]
        public string Message { get; set; }

        /// <summary>  
        /// Gets or sets is valid propertty.  
        /// </summary>  
        public bool isValid { get; set; }

        #endregion

    }
}