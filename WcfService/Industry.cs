//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WcfService
{
    using System;
    using System.Collections.Generic;
    
    public partial class Industry
    {
        public Industry()
        {
            this.ReatailerIndustries = new HashSet<ReatailerIndustry>();
        }
    
        public int Id { get; set; }
        public string Description { get; set; }
    
        public virtual ICollection<ReatailerIndustry> ReatailerIndustries { get; set; }
    }
}
