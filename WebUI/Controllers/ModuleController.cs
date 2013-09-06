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
    public class ModuleController : Cruder<Module, ModulesInput>
    {
        public ModuleController(ICrudService<Module> service, IMapper<Module, ModulesInput> v)
            : base(service, v)
        {
         
        }

        protected override string RowViewName
        {
            get { return "ListItems/Application"; }
        }

        [HttpPost]
        public string GetModules(int appId)
        {
            var query = from selectedModules in service.Where(o=>o.ApplicationId==appId && o.IsDeleted==false).AsEnumerable()
                        select new
                        {
                            selectedModules.Code,
                            selectedModules.Description
                        };

            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string sJson = oSerializer.Serialize(query);
            sJson = @"{""Data"":" + sJson + @",""success"":true}";
            return sJson;
        }

    }
}


