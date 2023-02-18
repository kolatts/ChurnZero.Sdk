using ChurnZero.Sdk.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace ChurnZero.Sdk.Models
{
    public class ChurnZeroAttributeModel : ChurnZeroModel, IValidatableObject
    {
        [Required]
        public virtual string Name { get; set; }
        [Required]
        public virtual string Value { get; set; }
        [Required]
        public virtual ChurnZeroEntities? Entity { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Entity == ChurnZeroEntities.Contact && string.IsNullOrWhiteSpace(ContactExternalId))
                yield return new ValidationResult($"A {nameof(ContactExternalId)} is required when the entity is {ChurnZeroEntities.Contact}");
        }
    }
}
