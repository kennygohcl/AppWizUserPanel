using System;

namespace dFrontierAppWizard.Core.Model
{
    public class Feedback : Entity
    {
        public int UserId { get; set; }
        public string Comments { get; set; }
        public DateTime DatePosted { get; set; }
    }
}