
using System.Linq;
using System.Web.Mvc;
using dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Service;
using dFrontierAppWizard.WebUI.Dto;
using dFrontierAppWizard.WebUI.Mappers;

namespace dFrontierAppWizard.WebUI.Controllers
{
    public class IndustryController : Cruder<Industry, IndustryInput>
    {
        private readonly IUserService _us;

        public IndustryController(ICrudService<Industry> service, IMapper<Industry, IndustryInput> v, IUserService us)
            : base(service, v)
        {
            this._us = us;
        }

        protected override string RowViewName
        {
            get { return "ListItems/Industry"; }
        }

      

       
    }

}