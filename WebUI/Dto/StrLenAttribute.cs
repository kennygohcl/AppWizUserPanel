using System.ComponentModel.DataAnnotations;
using dFrontierAppWizard.Resources;

namespace dFrontierAppWizard.WebUI.Dto
{
    public class StrLenAttribute : StringLengthAttribute
    {
        public StrLenAttribute(int maximumLength) : base(maximumLength)
        {
            ErrorMessageResourceName = "strlen";
            ErrorMessageResourceType = typeof (Mui);
        }
    }
}