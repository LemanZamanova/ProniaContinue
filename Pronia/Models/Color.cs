using System.ComponentModel.DataAnnotations;
using Pronia.Models.Base;

namespace Pronia.Models.Color

{
    public class Color : BaseEntity
    {
        [MaxLength(30)]
        public string Name { get; set; }


    }
}
