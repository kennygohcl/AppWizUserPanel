using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Omu.Encrypto;
using dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Repository;
using dFrontierAppWizard.Core.Service;
using dFrontierAppWizard.Data;
using dFrontierAppWizard.Service;
using dFrontierAppWizard.WebUI.Dto;
using dFrontierAppWizard.WebUI.Mappers;


namespace dFrontierAppWizard.WebUI.Controllers
{
    

    public class ProductCategoryController : Cruder<ProductCategory, ProductCategoryInput>
    {

        private new readonly IUserService userService;

        public ProductCategoryController(ICrudService<ProductCategory> service, IMapper<ProductCategory, ProductCategoryInput> v, IUserService userService)
            : base(service, v)
        {
            this.userService = userService;
        }

        public ProductCategoryInput productCategoryInput = new ProductCategoryInput();

        public override ActionResult Create()
        {
            return View(productCategoryInput);
        }

        public override ActionResult Index()
        {
            return View();
        }

        public  ActionResult IndexNoHeader()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public override ActionResult Create(ProductCategoryInput input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            var productCat = service.Create(new ProductCategory { Description = input.Description, UserId = userService.GetUserIdByName(HttpContext.User.Identity.Name) });
                service.Save();

            return Json(new {});
           
        }

        [HttpPost]
        public string GetProductCategoiesRetailerId(int id)
        {
            var query = from proCat in service
                        where proCat.UserId == id && proCat.IsDeleted == false
                        orderby proCat.Id
                        select new
                        {
                            proCat.Id,
                            proCat.Description
                        };
            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string sJson = oSerializer.Serialize(query);
            sJson = @"{""Data"":" + sJson + @",""success"":true}";
            return sJson;
        }


        protected override string RowViewName
        {
            get { return "ListItems/ProductCategory"; }
        }
    }
}
