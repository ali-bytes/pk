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
    
    public partial class PrivilegeAction
    {
        public PrivilegeAction()
        {
            this.ViewControls = new HashSet<ViewControl>();
        }
    
        public int Id { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public int PrivilegeId { get; set; }
    
        public virtual Privilege Privilege { get; set; }
        public virtual ICollection<ViewControl> ViewControls { get; set; }
    }
}
