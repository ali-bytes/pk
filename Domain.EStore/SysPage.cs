//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Domain.EStore
{
    using System;
    using System.Collections.Generic;
    
    public partial class SysPage
    {
        public SysPage()
        {
            this.LanguagePages = new HashSet<LanguagePage>();
            this.PageTools = new HashSet<PageTool>();
            this.SeoPageTools = new HashSet<SeoPageTool>();
        }
    
        public int PageId { get; set; }
        public string PageName { get; set; }
    
        public virtual ICollection<LanguagePage> LanguagePages { get; set; }
        public virtual ICollection<PageTool> PageTools { get; set; }
        public virtual ICollection<SeoPageTool> SeoPageTools { get; set; }
    }
}
