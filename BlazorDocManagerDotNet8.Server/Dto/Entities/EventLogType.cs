using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorDocManagerDotNet8.Server.Dto.Entities
{
    [Table("EventLogType" , Schema = "log")]
    public class EventLogType
    {
        [Key, Required]
        public Guid LogTypeID { get; set; }
        [Display(Name = "نوع رخداد")]
        [Required(ErrorMessage = "ورود عنوان {0} الزامی است.")]
        [MaxLength(250, ErrorMessage = "حداکثر طول عنوان {2} کاراکتر می باشد.")]
        public String EventTypeDescription { get; set; }

        [ConcurrencyCheck]
        public Guid RowVersion { get; set; }
    }
}
