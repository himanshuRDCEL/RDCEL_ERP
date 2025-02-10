using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Common.Enums
{
    public enum QuestionerLOV
    {
        [Description("Upper Boolen")]
        Upper_Boolen = 1,
        [Description("Lower Boolen")]
        Lower_Boolen = 2,
        [Description("Numeric")]
        Numeric = 3,
        [Description("Upper Range")]
        Upper_Range = 4,
        [Description("Lower Range")]
        Lower_Range = 5,
        [Description("Yes")]
        Upper_Yes = 6,
        [Description("No")]
        Upper_No = 7,
        [Description("Yes")]
        Lower_Yes = 8,
        [Description("No")]
        Lower_No = 9,
        [Description("0-1")]
        Zero_To_One = 10,
        [Description("2")]
        Two = 11,
        [Description("3")]
        Three = 12,
        [Description("4")]
        Four = 13,
        [Description("5")]
        Five = 14,
        [Description("6")]
        Six = 15,
        [Description("7")]
        Seven = 16,
        [Description("8")]
        Eight = 17,
        [Description("9")]
        Nine = 18,
        [Description("10+")]
        TenPlus = 19,
        [Description("0%")]
        Zero_Percentage = 20,
        [Description("Upto 50%")]
        Upto_Fifty_Percentage = 21,
        [Description("51-90%")]
        FiftyOne_Ninty_Percentage = 22,
        [Description("More than 91%")]
        More_Than_Ninty_Percentage = 23,
        [Description("0%")]
        Lower_Zero_Percentage = 24,
        [Description("Upto 10%")]
        Upto_Ten_Percentage = 25,
        [Description("11-25%")]
        Eleven_To_TwentyFive_Percentage = 26,
        [Description("More than 25%")]
        More_Than_TwentyFive = 27,
        [Description("Rust Level")]
        Rust_Level = 28,
        [Description("No Rust")]
        No_Rust = 29,
        [Description("Low Rust")]
        Low_Rust = 30,
        [Description("Heavy Rust")]
        Heavy_Rust = 31,

    }
   
}
