using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Service;
using dFrontierAppWizard.WebUI.Dto;
using dFrontierAppWizard.WebUI.Mappers;
using dFrontierAppWizard.WebUI.Utils;

namespace dFrontierAppWizard.WebUI.Controllers
{
    public class BranchController : Cruder<Branch, BranchInput>
    {
        private new readonly IUserService us;
      
        public BranchController(ICrudService<Branch> service, IMapper<Branch, BranchInput> v, IUserService us)
            : base(service, v)
        {
            this.us = us;
        }

        public BranchInput newBranchInput = new BranchInput();

        public override ActionResult Create()
        {
            ViewBag.UserId = us.GetUserIdByName(HttpContext.User.Identity.Name);
            return View(newBranchInput);
        }

        [HttpPost]
        public string GetRetailerAddressesByRetailerId(int retailerId)
        {
            var addresses = service.Where(o => o.UserId.Equals(retailerId)).AsEnumerable();
            var query = from selectedBranches in addresses.AsEnumerable()
                        select new
                        {
                            selectedBranches.Id,
                            selectedBranches.Address,
                            selectedBranches.ZipCode
                        };

            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string sJson = oSerializer.Serialize(query);
            sJson = @"{""Data"":" + sJson + @",""success"":true}";
            return sJson;
        }



      /*  [HttpPost]
        public override ActionResult Create(BranchInput input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            input.UserId = us.GetUserIdByName(HttpContext.User.Identity.Name);
         

            var list = service.Where(o => o.BranchName.Equals(input.BranchName) && o.UserId.Equals(input.UserId)).SingleOrDefault();

            if (list != null)
            {
                ViewBag.BranchDuplicate = "Yes";
                return View(input);

            }
            
            var branch = service.Create(new Branch
            {
                BranchName = input.BranchName,
                CountryId = input.CountryId,
                Location = input.Location,
                UserId = input.UserId
            });
            service.Save();

            return RedirectToAction("Index","Branch");
          
        }*/

      
        protected override string RowViewName
        {
            get { return "ListItems/Branch"; }
        }
    }
}