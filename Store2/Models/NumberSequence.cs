﻿using System.ComponentModel.DataAnnotations;

namespace Store2.Models
{
    public class NumberSequence
    {
        public int NumberSequenceId { get; set; }
        [Required]
        public string NumberSequenceName { get; set; }
        [Required]
        public string Module { get; set; }
        [Required]
        public string Prefix { get; set; }
        public int LastNumber { get; set; }
    }
}
