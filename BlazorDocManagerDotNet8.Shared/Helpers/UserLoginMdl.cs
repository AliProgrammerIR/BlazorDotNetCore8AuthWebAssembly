using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDocManagerDotNet8.Shared.Helpers
{
    public class UserLoginMdl
    {
        [EmailAddress]
        [Display(Name = "ایمیل"), Required(ErrorMessage = "ورود ایمیل الزامی می باشد."), MinLength(5, ErrorMessage = "حداقل کاراکترهای مجاز 2 می باشد"), MaxLength(150, ErrorMessage = "حداکثر طول ایمیل 150 حرف می باشد")]
        public String Email { get; set; }
        [PasswordPropertyText]
        [Display(Name = "کلمه عبور"), Required(ErrorMessage = "ورود کلمه عبور الزامی می باشد."), MinLength(6, ErrorMessage = "حداقل کاراکترهای مجاز 2 می باشد"), MaxLength(150, ErrorMessage = "حداکثر طول کلمه عبور 150 حرف می باشد")]
        public String Password { get; set; }
    }
    public class UserRegMdl
    {
        [EmailAddress]
        [Display(Name = "ایمیل"), Required(ErrorMessage = "ورود ایمیل الزامی می باشد."), MinLength(5, ErrorMessage = "حداقل کاراکترهای مجاز 2 می باشد"), MaxLength(150, ErrorMessage = "حداکثر طول ایمیل 150 حرف می باشد")]
        public String Email { get; set; }
        [PasswordPropertyText]
        [Display(Name = "کلمه عبور"), Required(ErrorMessage = "ورود کلمه عبور الزامی می باشد."), MinLength(6, ErrorMessage = "حداقل کاراکترهای مجاز 2 می باشد"), MaxLength(150, ErrorMessage = "حداکثر طول کلمه عبور 150 حرف می باشد")]
        public String Password { get; set; }
        [Display(Name = "نام"), Required(ErrorMessage = "ورود نام الزامی می باشد."), MinLength(2, ErrorMessage = "حداقل کاراکترهای مجاز 2 می باشد"), MaxLength(150, ErrorMessage = "حداکثر طول نام 150 حرف می باشد")]
        public String FirstName { get; set; }
        [Display(Name = "نام خانوادگی"), Required(ErrorMessage = "ورود نام خانوادگی الزامی می باشد."), MinLength(2, ErrorMessage = "حداقل کاراکترهای مجاز 2 می باشد"), MaxLength(150, ErrorMessage = "حداکثر طول نام خانوادگی 150 حرف می باشد")]
        public String LastName { get; set; }
    }
}