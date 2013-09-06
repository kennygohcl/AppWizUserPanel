namespace dFrontierAppWizard.Core.Model
{
    public class Branch : DelEntity
    {
        public string BranchName { get; set; }
        public string Address { get; set; }
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }
        public int UserId { get; set; }
        public bool IsPublished { get; set; }
        public string ZipCode { get; set; }
    }
}