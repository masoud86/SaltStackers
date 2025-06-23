using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Common.Enums
{
    public enum LogType
    {
        [Display(Name = "Error")]
        Error = 1,

        [Display(Name = "Warning")]
        Warning = 2,

        [Display(Name = "Info")]
        Info = 3,

        [Display(Name = "Debug")]
        Debug = 4
    }
}
