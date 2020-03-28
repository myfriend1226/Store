using System.ComponentModel.DataAnnotations;

namespace Store2.Models
{
    public class CashBank : BaseModel
    {
        public int CashBankId { get; set; }
        [Display(Name = "Cash / Bank Name")]
        public string CashBankName { get; set; }
        public string Description { get; set; }
    }

    public class CashBankViewModel
    {
        public int CashBankId { get; set; }
        [Display(Name = "Cash / Bank Name")]
        public string CashBankName { get; set; }
        public string Description { get; set; }
    }
}
