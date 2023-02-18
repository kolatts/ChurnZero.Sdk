using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace ChurnZero.Sdk.Models
{
    public class ChurnZeroEventModel : ChurnZeroModel
    {
        [Required]
        public override string ContactExternalId { get; set; }

        [Required]
        public virtual string EventName { get; set; }
        public virtual DateTime? EventDate { get; set; }

        public virtual string Description { get; set; }
        public virtual int? Quantity { get; set; }

        public virtual Dictionary<string, string> CustomFields { get; set; } = new Dictionary<string, string>();
    }
}
