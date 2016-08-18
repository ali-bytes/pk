using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ui.EStore.Models
{
    public class PrivilegeRule
    {
        public int SysRoleId { get; set; }

        public int SysPrivilegeId { get; set; }

        public bool SysPrivilageState { get; set; }
    }
}