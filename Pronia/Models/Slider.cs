using System.ComponentModel.DataAnnotations;
using Pronia.Models.Base;

namespace Pronia.Models
{
    public class Slider : BaseEntity
    {

        [MaxLength(100, ErrorMessage = "slide Title must be 100 characters or fewer")]
        public string Title { get; set; }
        public string SubTitle { get; set; }

        [MaxLength(300, ErrorMessage = "slide description must be 300 characters or fewer")]
        public string Description { get; set; }
        public string Image { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Order cannot be less than 1.")]
        public int Order { get; set; }


    }
}
