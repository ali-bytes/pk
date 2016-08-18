using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ui.EStore.Models
{
    public class RegisterCustomer
    {

        [Required(ErrorMessage = "أدخل أسم المستخدم")]
        [StringLength(250)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "أدخل كلمة المرور")]
        [StringLength(250)]
        public string Password { get; set; }

        [Required(ErrorMessage = "أدخل تاكيد كلمة المرور")]
        [StringLength(250)]
      
        public string ConfirmPassword { get; set; }


        [Required(ErrorMessage = "أدخل البريد الالكترونى")]
        [StringLength(250)]
        //[Email(ErrorMessage = "أدخل بريد الكترونى صحيح")]
        public string Email { get; set; }
    }
}