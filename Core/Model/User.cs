using System;
using System.Collections.Generic;

namespace dFrontierAppWizard.Core.Model
{
    public class User : DelEntity
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime DateCreated { get; set; }
        public int UserTypeId { get; set; }
        public string PaymentStatus { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<Industry> Industries { get; set; }
    }
}