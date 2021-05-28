using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using mcg_load.Code.Helpers;

namespace mcg_load.Models.FileUpload
{
    public class FileUploadResult
    {
        #region Properties  
        
        /// <summary>  
        /// Gets or sets message property.  
        /// </summary>
        [Required]
        [Display(Name = "Nombre de la Carga")]
        public string Nombre { get; set; }

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

        [Display(Name = "Respuesta de Servicio")]
        public string MessageResult { get; set; }

        public Esc_File_Upload FileUpload { get; set; }

        public List<FileUploadError> FileUploadErrors { get; set; }
        #endregion
    }
}