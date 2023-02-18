using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ChurnZero.Sdk.Constants;

namespace ChurnZero.Sdk.Models
{
    public abstract class ChurnZeroModel
    {
        [Required]
        public string AccountExternalId { get; set; }
        public virtual string ContactExternalId { get; set; }

    }
}
