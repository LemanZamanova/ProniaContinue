using System.ComponentModel.DataAnnotations;
using Pronia.Models.Base;

namespace Pronia.Models
{
    public class Category : BaseEntity
    {
        [MinLength(3, ErrorMessage = "3den qisa olmaz")]
        [MaxLength(30)]
        public string Name { get; set; }


        public List<Product>? Products { get; set; }

    }
}
