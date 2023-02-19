using ChurnZero.Sdk.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChurnZero.Sdk.Models
{

    public sealed class ChurnZeroAttributeModel : ChurnZeroModel, IValidatableObject
    {
        /// <summary>
        /// Supports custom fields for name. Dates should be provided in ISO-8601 e.g. DateTime.toString("O")
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="accountExternalId"></param>
        /// <param name="contactExternalId"></param>
        public ChurnZeroAttributeModel( string name, string value, EntityTypes entity, string accountExternalId, string contactExternalId = null)
        {
            EntityType = entity;
            Name = name;
            Value = value;
            AccountExternalId = accountExternalId;
            ContactExternalId = contactExternalId;
        }
        /// <summary>
        /// Standard fields for contacts.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="contactExternalId"></param>
        /// <param name="accountIdentifier"></param>
        /// <param name="value"></param>
        public ChurnZeroAttributeModel(string accountIdentifier, string contactExternalId, StandardContactFields field, string value)
        {
            AccountExternalId = accountIdentifier;
            ContactExternalId = contactExternalId;
            EntityType = EntityTypes.Contact;
            Name = $"attr_{field}";
            Value = value;
        }
        /// <summary>
        /// Standard string type fields for Accounts
        /// </summary>
        /// <param name="field"></param>
        /// <param name="accountExternalId"></param>
        /// <param name="value"></param>
        public ChurnZeroAttributeModel(string accountExternalId, StandardAccountFields field,  string value)
        {
            AccountExternalId = accountExternalId;
            EntityType = EntityTypes.Account;
            Name = $"attr_{field}";
            Value = value;
        }
        /// <summary>
        /// Standard date type fields for Accounts (Start Date, End Date, Next Renewal Date)
        /// </summary>
        /// <param name="field"></param>
        /// <param name="accountExternalId"></param>
        /// <param name="date"></param>
        public ChurnZeroAttributeModel(string accountExternalId, StandardAccountFields field,  DateTime date)
        : this(accountExternalId, field, date.ToString("O")) { }

        public ChurnZeroAttributeModel(string accountExternalId, StandardAccountFields field, double numberAsDouble)
        : this(accountExternalId, field,  numberAsDouble.ToString("N2")) { }

        public ChurnZeroAttributeModel(string accountExternalId, StandardAccountFields field, int numberAsInt)
        : this(accountExternalId, field, numberAsInt.ToString()) { }



        [Required]
        public string Name { get; set; }
        [Required]
        public string Value { get; set; }
        [Required]
        public EntityTypes? EntityType { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EntityType == EntityTypes.Contact && string.IsNullOrWhiteSpace(ContactExternalId))
                yield return new ValidationResult($"A {nameof(ContactExternalId)} is required when the entity is {EntityTypes.Contact}");
        }


    }
}
