using ChurnZero.Sdk.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChurnZero.Sdk.Models
{
    public class ChurnZeroAttributeModel : ChurnZeroModel, IValidatableObject
    {
        internal ChurnZeroAttributeModel()
        {

        }
        public ChurnZeroAttributeModel(StandardContactFields field, string contactExternalId, string accountIdentifier, string value)
        {
            AccountExternalId = accountIdentifier;
            ContactExternalId = contactExternalId;
            EntityType = EntityTypes.Contact;
            Name = $"attr_{field}";
            Value = value;
        }
        public ChurnZeroAttributeModel(StandardAccountFields field, string accountExternalId, string value)
        {
            AccountExternalId = accountExternalId;
            EntityType = EntityTypes.Account;
            Name = $"attr_{field}";
            Value = value;
        }

        public ChurnZeroAttributeModel(StandardAccountFields field,string accountExternalId, DateTime date)
        :this(field, accountExternalId, date.ToString("O")) { }

        public ChurnZeroAttributeModel(StandardAccountFields field,string accountExternalId, double numberAsDouble)
        :this(field, accountExternalId, numberAsDouble.ToString("N2")) { }

        public ChurnZeroAttributeModel(StandardAccountFields field,string accountExternalId, int numberAsInt)
        :this(field,accountExternalId, numberAsInt.ToString()) { }

   

        [Required]
        public string Name { get; set; }
        [Required]
        public string Value { get; set; }
        [Required]
        public virtual EntityTypes? EntityType { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EntityType == EntityTypes.Contact && string.IsNullOrWhiteSpace(ContactExternalId))
                yield return new ValidationResult($"A {nameof(ContactExternalId)} is required when the entity is {EntityTypes.Contact}");
        }

     
    }
}
