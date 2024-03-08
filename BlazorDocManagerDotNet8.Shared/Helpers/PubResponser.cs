using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDocManagerDotNet8.Shared.Helpers
{
    public class PubResponser
    {
        public String StrMessage { get; set; }
        public String StrErrorMessage { get; set; }
        public ResponserState ResponsState { get; set; }
    }

    public enum ResponserState
    {
        Successful,
        Warning,
        TwoVerification,
        Fail,
        NoRecodsFound,
        /// <summary>
        /// زمانی که نیو زده  می شود و نیاز به نمایش اسپینر داریم
        /// </summary>
        JustStarted
    }
}
