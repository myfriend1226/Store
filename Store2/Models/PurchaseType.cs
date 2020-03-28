using System.ComponentModel.DataAnnotations;

namespace Store2.Models
{
    public class PurchaseType : BaseModel
    {
        public int PurchaseTypeId { get; set; }
        [Required]
        public string PurchaseTypeName { get; set; }
        public string Description { get; set; }
    }

    public class PurchaseTypeViewModel
    {
        public int PurchaseTypeId { get; set; }
        [Required]
        public string PurchaseTypeName { get; set; }
        public string Description { get; set; }
    }
}
