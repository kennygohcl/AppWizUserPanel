using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dFrontierAppWizard.Core.Model
{
    public class Lead : DelEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Address { get; set; }
        public string ReferredBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string Mobile { get; set; }
        
    }
}

