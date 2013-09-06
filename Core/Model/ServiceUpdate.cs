using System;

namespace dFrontierAppWizard.Core.Model
{
    public class ServiceUpdate : DelEntity
    {
        public int ApplicationId { get; set; }
        public DateTime ProductCategories { get; set; }
        public DateTime Products { get; set; }
        public DateTime Promotions { get; set; }
        public DateTime Consumers { get; set; }
        public DateTime ConsumerFeedbacks { get; set; }
        public DateTime ConsumerLoyalties { get; set; }
        public DateTime Branches { get; set; }
        public DateTime LastModifiedAll { get; set; }
    }
}