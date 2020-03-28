using System.ComponentModel.DataAnnotations;

namespace Store2.Models
{
    public class ProductType : BaseModel
    {
        public int ProductTypeId { get; set; }
        [Required]
        public string ProductTypeName { get; set; }
        public string Description { get; set; }
    }

    public class ProductTypeViewModel
    {
        public int ProductTypeId { get; set; }
        [Required]
        public string ProductTypeName { get; set; }
        public string Description { get; set; }
    }
}
