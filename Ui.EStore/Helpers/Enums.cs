using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ui.EStore.Helpers
{
    public class Enums
    {
        public enum OperationStatus
        {
            Ok = 0,
            Error = 1,
        }

        public enum PrivilegeType
        {
            Action = 1,
            ViewControl = 2,
        }
        public enum Lang
        {
            Ar = 1,
            En = 2
        }
    }
}