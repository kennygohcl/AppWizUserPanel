using System;
using System.Collections.Generic;

namespace dFrontierAppWizard.Core.Model
{
    public class Promotion : DelEntity
    {
        public decimal Price { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public bool IsPublished { get; set; }
        public int DiscountPercentage { get; set; }
        public bool IsActive { get; set; }
    }
}