using System;

namespace dFrontierAppWizard.Core.Model
{
    public class PushNotificationRegUser : DelEntity
    {
		public int UserId { get; set; }
		public string RegId { get; set; }
		public string DeviceId { get; set; }
		public DateTime CreatedTime { get; set; }	 
    }
}
