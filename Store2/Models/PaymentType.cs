using System.ComponentModel.DataAnnotations;

namespace Store2.Models
{
    public class PaymentType : BaseModel
    {
        public int PaymentTypeId { get; set; }
        [Required]
        public string PaymentTypeName { get; set; }
        public string Description { get; set; }
    }

    public class PaymentTypeViewModel
    {
        public int PaymentTypeId { get; set; }
        [Required]
        public string PaymentTypeName { get; set; }
        public string Description { get; set; }
    }
}
