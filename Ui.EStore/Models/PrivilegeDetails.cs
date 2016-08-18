using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ui.EStore.Models
{
    public class PrivilegeDetails
    {
        public decimal SysPrivilegeActionId { get; set; }

        public decimal SysPrivilegeId { get; set; }

        public decimal SysPrivilegeCategoryId { get; set; }

        public string SysControllerId { get; set; }

        public string SysActionId { get; set; }

        public string SysPrivilegeCategoryName { get; set; }

        public string SysPrivilegeDisplayName { get; set; }

        public bool SysPrivilageState { get; set; }

    }
}