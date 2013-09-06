using System;

namespace dFrontierAppWizard.Core
{
    [Serializable]
    public class AppWizardException : Exception
    {
        public AppWizardException(string message)
            : base(message)
        {
        }
    }
}