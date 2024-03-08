using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BlazorDocManagerDotNet8.Server.Dto.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "نام"), Required(ErrorMessage = "ورود نام الزامی می باشد."), MinLength(2, ErrorMessage = "حداقل کاراکترهای مجاز 2 می باشد"), MaxLength(150, ErrorMessage = "حداکثر طول نام 150 حرف می باشد")]
        public String FirstName { get; set; }
        [Display(Name = "نام خانوادگی"), Required(ErrorMessage = "ورود نام خانوادگی الزامی می باشد."), MinLength(2, ErrorMessage = "حداقل کاراکترهای مجاز 2 می باشد"), MaxLength(150, ErrorMessage = "حداکثر طول نام خانوادگی 150 حرف می باشد")]
        public String LastName { get; set; }
        [Timestamp]
        public Byte[] RowVersion { get; set; }
    }
}
