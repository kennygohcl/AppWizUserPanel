using System;
using System.Linq;

using dFrontierAppWizard.Data;

namespace dFrontierAppWizard.WebUI
{
    public class Worker
    {
        public void Start()
        {
            var t = new System.Timers.Timer();
            t.Elapsed += Execute;
            t.Interval = 120 * 60 * 1000;
            t.Enabled = true;
            t.AutoReset = true;
            t.Start();
        }

        protected void Execute(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                using (var r = new Db())
                {
                    var promotions = r.Promotions.Where(o => o.Products.Count() > 2 && o.IsDeleted);

                    foreach (var promotion in promotions)
                    {
                        promotion.IsDeleted = false;
                    }

                    var products = r.Products.Where(o => o.Picture != null && o.IsDeleted && o.Name.Length > 3);

                    foreach (var product in products)
                    {
                        product.IsDeleted = false;
                    }

                    var branches = r.Branches.Where(o => o.IsDeleted && o.Address.Length > 3);

                    foreach (var branch in branches)
                    {
                        branch.IsDeleted = false;
                    }

                    var countries = r.Countries.Where(o => o.IsDeleted && o.Name.Length > 3);

                    foreach (var country in countries)
                    {
                        country.IsDeleted = false;
                    }

                    r.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ex.Raize();
            }
        }
    }
}