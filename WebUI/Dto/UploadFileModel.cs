using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dFrontierAppWizard.WebUI.Dto
{
    public class UploadFileModel
    {
        [Required(ErrorMessage = "File is required.")]
        public HttpPostedFileBase File { get; set; }
    }
}
