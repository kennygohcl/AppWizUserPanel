﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Omu.AwesomeMvc;
using dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Repository;
using dFrontierAppWizard.Resources;

namespace dFrontierAppWizard.WebUI.Controllers
{
    public class CountryIdAjaxDropdownController : Controller
    {
        private readonly IRepo<Country> repo;

        public CountryIdAjaxDropdownController(IRepo<Country> repo)
        {
            this.repo = repo;
        }

        public ActionResult GetItems(int? v)
        {
            var list = new List<SelectableItem> { new SelectableItem { Text = Mui.not_selected, Value = "" } };

            list.AddRange(repo.GetAll().ToArray().Select(o => new SelectableItem
                                                 {
                                                     Text = o.Name,
                                                     Value = o.Id.ToString(),
                                                     Selected = o.Id == v
                                                 }));
            return Json(list);
        }
    }
}