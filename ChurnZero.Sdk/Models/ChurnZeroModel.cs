using System.ComponentModel.DataAnnotations;

namespace ChurnZero.Sdk.Models
{
    public abstract class ChurnZeroModel
    {
        [Required]
        public string AccountExternalId { get; set; }
        public virtual string ContactExternalId { get; set; }

    }
}
