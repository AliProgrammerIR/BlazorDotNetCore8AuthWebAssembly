using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDocManagerDotNet8.Shared.Helpers
{
    public class UserToken
    {
        public UserToken()
        {
            Responser = new PubResponser();
        }
        public String Token { get; set; }
        public DateTime ExpireDate { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public PubResponser? Responser { get; set; }
    }
}
