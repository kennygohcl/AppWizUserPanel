using System;

namespace dFrontierAppWizard.Core.Model
{
    public class Consumer : DelEntity
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public long Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime DateRegistered { get; set; }
        public string Picture { get; set; }
        public string Token { get; set; }
        public string DeviceId { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        
    }
}