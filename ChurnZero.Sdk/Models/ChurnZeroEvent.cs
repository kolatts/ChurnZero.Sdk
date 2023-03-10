using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChurnZero.Sdk.Models
{
    public class ChurnZeroEvent 
    {
        [Required]
        public string AccountExternalId { get; set; }
        [Required]
        public string ContactExternalId { get; set; }

        [Required]
        public virtual string EventName { get; set; }
        public virtual DateTime? EventDate { get; set; }

        public virtual string Description { get; set; }
        public virtual int? Quantity { get; set; }
        public virtual bool AllowDupes { get; set; }
        /// <summary>
        /// Use the intended display name in Churn Zero as the key.
        /// </summary>
        //public Dictionary<string,string> CustomFields { get; set; }
    }
}
