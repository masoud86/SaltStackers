using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Common.Enums
{
    public enum MediaType
    {
        [Display(Name = "Image")]
        Image = 1,

        [Display(Name = "Document")]
        Document = 2
    }
}
