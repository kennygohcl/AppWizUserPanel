using System.Collections.Generic;

namespace dFrontierAppWizard.Core.Model
{
    public class Industry : DelEntity
    {
        public string Description { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}