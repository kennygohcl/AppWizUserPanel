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

    public class AdController : Cruder<Ad, AdCreateInput>
    {
        public AdCreateInput newAdvertismentInput = new AdCreateInput();

        public AdController(ICrudService<Ad> service, IMapper<Ad, AdCreateInput> v)
            : base(service, v)
        {
        }

       //public ActionResult Create()
       //{
       //    //List<int> roleList = new List<int>();
       //    //roleList.Add(3);
       //    //newUserCreateInput.Roles = roleList;
       //    return View(newAdvertismentInput);
       //}


        [HttpPost]
        public string GetAdsByAppId(int appId)
        {
            var advertisment = service.Where(o => o.ApplicationId.Equals(appId) && o.Start >= DateTime.Now && o.End <= DateTime.Now).AsEnumerable();
            var query = from selectedAd in advertisment.AsEnumerable()
                        select new
                        {
                            selectedAd.Id,
                            selectedAd.ApplicationId,
                            selectedAd.WebSiteReference,
                            selectedAd.Banner
                        };

            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string sJson = oSerializer.Serialize(query);
            sJson = @"{""Data"":" + sJson + @",""success"":true}";
            return sJson;

        }

        protected override string RowViewName
        {
            get { return "ListItems/Ad"; }
        }

    }


}
