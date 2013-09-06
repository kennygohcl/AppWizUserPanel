using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Service;
using dFrontierAppWizard.WebUI.Dto;
using dFrontierAppWizard.WebUI.Mappers;

namespace dFrontierAppWizard.WebUI.Controllers
{
    public class MediaDocumentController : Cruder<MediaDocument, MediaDocumentInput>
    {
      //  private readonly IUserService _us;

        public MediaDocumentController(ICrudService<MediaDocument> service, IMapper<MediaDocument, MediaDocumentInput> v)
            : base(service, v)
        {
           // this._us = us;
        }

        protected override string RowViewName
        {
            get { return "ListItems/MediaDocument"; }
        }
    }
}
