using ChurnZero.Sdk.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChurnZero.Sdk.Models
{

    public sealed class ChurnZeroAttribute :  IValidatableObject
    {
        [Required]
        public string AccountExternalId { get; set; }
        public string ContactExternalId { get; set; }
        /// <summary>
        /// Supports custom fields for name.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="accountExternalId"></param>
        /// <param name="contactExternalId"></param>
        public ChurnZeroAttribute(string name, string value, EntityTypes entity, string accountExternalId, string contactExternalId = null)
        {
            EntityType = entity;
            Name = name;
            Value = value;
            AccountExternalId = accountExternalId;
            ContactExternalId = contactExternalId;
        }
        /// <summary>
        /// Supports custom fields for name. Dates should be provided in ISO-8601 e.g. DateTime.toString("O")
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="accountExternalId"></param>
        /// <param name="contactExternalId"></param>
        public ChurnZeroAttribute(string name, DateTime value, EntityTypes entity, string accountExternalId, string contactExternalId = null)
        : this(name, value.ToString("O"), entity, accountExternalId, contactExternalId)
        {

        }
        /// <summary>
        /// Standard fields for contacts.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="contactExternalId"></param>
        /// <param name="accountIdentifier"></param>
        /// <param name="value"></param>
        public ChurnZeroAttribute(string accountIdentifier, string contactExternalId, StandardContactFields field, string value)
        {
            AccountExternalId = accountIdentifier;
            ContactExternalId = contactExternalId;
            EntityType = EntityTypes.Contact;
            Name = $"{field}";
            Value = value;
        }
        /// <summary>
        /// Standard string type fields for Accounts
        /// </summary>
        /// <param name="field"></param>
        /// <param name="accountExternalId"></param>
        /// <param name="value"></param>
        public ChurnZeroAttribute(string accountExternalId, StandardAccountFields field, string value)
        {
            AccountExternalId = accountExternalId;
            EntityType = EntityTypes.Account;
            Name = $"{field}";
            Value = value;
        }

        /// <summary>
        /// Standard date type fields for Accounts (Start Date, End Date, Next Renewal Date)
        /// </summary>
        /// <param name="field"></param>
        /// <param name="accountExternalId"></param>
        /// <param name="date"></param>
        public ChurnZeroAttribute(string accountExternalId, StandardAccountFields field, DateTime? date)
            : this(accountExternalId, field, date?.ToString("O")) { }


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
