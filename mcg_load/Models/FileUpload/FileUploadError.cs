using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mcg_load.Models.FileUpload
{
    public class FileUploadError
    {
        [Required]
        [Display(Name = "Nombre de la Hoja")]
        public string Nombre_Hoja { get; set; }

        /// <summary>  
        /// Gets or sets message property.  
        /// </summary>  
        [Required]
        [Display(Name = "Descripción del error")]
        public string Message { get; set; }

    }
}