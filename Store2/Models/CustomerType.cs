using System.ComponentModel.DataAnnotations;

namespace Store2.Models
{
    public class CustomerType : BaseModel
    {
        public int CustomerTypeId { get; set; }
        [Required]
        public string CustomerTypeName { get; set; }
        public string Description { get; set; }
    }

    public class CustomerTypeViewModel
    {
        public int CustomerTypeId { get; set; }
        [Required]
        public string CustomerTypeName { get; set; }
        public string Description { get; set; }
    }
}
