using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Ui.EStore.Models
{
    public class CustomerLogin
    {
        [Required(ErrorMessage = "أدخل كلمة المرور")]
        [StringLength(250)]
        public string Password { get; set; }

        [Required(ErrorMessage = "أدخل البريد الالكترونى")]
        [StringLength(250)]
        [RegularExpression(@"/[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}/igm",ErrorMessage = "أدخل بريد الكترونى صحيح")]
        public string Email { get; set; }
    }
}