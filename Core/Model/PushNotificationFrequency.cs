using System;

namespace dFrontierAppWizard.Core.Model
{
    public class PushNotificationFrequency : DelEntity
    {
        public int ApplicationId { get; set; }
        public DateTime Frequency { get; set; }
      
    }
}