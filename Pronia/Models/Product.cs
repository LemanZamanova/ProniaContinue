using Pronia.Models.Base;

namespace Pronia.Models
{
    public class Product : BaseEntity
    {


        public string SKU { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public List<ProductImage> ProductImage { get; set; }


    }
}
