
using System.Collections.Generic;
using dFrontierAppWizard.Core.Service;


namespace dFrontierAppWizard.Core.Model
{
    public class ProductCategory : DelEntity
    {
       

        public string Description { get; set; }
        public int UserId { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public bool IsPublished { get; set; }
    }
}

