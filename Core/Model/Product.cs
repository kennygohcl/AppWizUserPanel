using System.Collections.Generic;
using dFrontierAppWizard.Core.Service;

namespace dFrontierAppWizard.Core.Model
{
    public class Product : DelEntity
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Promotion> Promotions { get; set; }
        public int ProductCategoryId { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
        public int Userid { get; set; }
        public decimal Price { get; set; }
        public string Picture { get; set; }
        public bool IsPublished { get; set; }
        public int LikesCounter { get; set; }
    }
}