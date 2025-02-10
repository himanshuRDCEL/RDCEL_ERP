using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Common.Enums
{
    public enum QCCommentViewModelEnum
    {
        [Description("Excellent")]
        Excellent = 1,

        [Description("Good")]
        Good = 2,

        [Description("Average")]
        Average = 3,

        [Description("Not Working")]  
        //[Description("Non Working")]
        NotWorking = 4,

        [Description("Working")]
        Working = 5,

        [Description("Heavily Used")]
        HeavilyUsed = 6,

        [Description("Non Working")]
        NonWorking = 7,
    }
    public enum EvcPartnerPreferenceEnum
    {
        [Description("Excellent")]
        Excellent = 1,

        [Description("Good")]
        Good = 2,

        [Description("Average")]
        Average = 3,

        [Description("Not Working")]
        Not_Working = 4,
    }
}
