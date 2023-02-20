using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ChurnZero.Sdk.Models
{
    public class ChurnZeroBatchEvent
    {
        [Required]
        public string AccountExternalId { get; set; }
        [Required]
        public string ContactExternalId { get; set; }

        [Required]
        public string EventName { get; set; }
        [Required]
        public DateTime? EventDate { get; set; }

        public string Description { get; set; }
        public int? Quantity { get; set; }

        public Dictionary<string, string> CustomFields { get; set; } = new Dictionary<string, string>();
    }
}
