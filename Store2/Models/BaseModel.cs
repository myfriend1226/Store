using System;

namespace Store2.Models
{
    public class BaseModel
    {
        //audit trail
        public string CreateBy { get; set; }
        public DateTime CreateAtUtc { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateAtUtc { get; set; }
    }
}
