using System;
using System.Collections.Generic;

namespace dFrontierAppWizard.Core.Model
{
    public class MediaDocument : DelEntity
    {
        public string DocName { get; set; }
        public DateTime DateCreated { get; set; }
        public string MediaType { get; set; }
        public string Module { get; set; }
        public int ApplicationId { get; set; }
        
    }
}