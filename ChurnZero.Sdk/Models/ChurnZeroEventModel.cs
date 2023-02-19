using System;
using System.ComponentModel.DataAnnotations;

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
        public virtual bool AllowDupes { get; set; }
    }
}
