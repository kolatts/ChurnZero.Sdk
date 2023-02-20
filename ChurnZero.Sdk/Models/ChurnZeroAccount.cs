using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ChurnZero.Sdk.Constants;

namespace ChurnZero.Sdk.Models
{
    public interface IChurnZeroAccount
    {
        string AccountExternalId { get; set; }
    }

    public class ChurnZeroAccount : IChurnZeroAccount
    {
        [Required]
        public string AccountExternalId { get; set; }

        public string Name { get; set; }
        public DateTime? NextRenewalDate { get; set; }

        public bool? IsActive { get; set; }
        public string BillingAddressLine1 { get; set; }
        public string BillingAddressLine2 { get; set; }
        public string BillingAddressCity { get; set; }
        public string BillingAddressState { get; set; }
        public string BillingAddressZip { get; set; }
        public string BillingAddressCountry { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int? LicenseCount { get; set; }
        public string OwnerUserAccount { get; set; }
        public string ParentAccountExternalId { get; set; }
        public Dictionary<string, string> CustomFields { get; set; } = new Dictionary<string, string>();
        internal IEnumerable<ChurnZeroAttribute> ToAttributes(bool prefixCustomFields = false)
        {
            return new List<ChurnZeroAttribute>()
            {
                new ChurnZeroAttribute(AccountExternalId, StandardAccountFields.Name, Name),
                new ChurnZeroAttribute(AccountExternalId, StandardAccountFields.NextRenewalDate, NextRenewalDate),
                new ChurnZeroAttribute(AccountExternalId, StandardAccountFields.IsActive, IsActive?.ToString()),
                new ChurnZeroAttribute(AccountExternalId, StandardAccountFields.BillingAddressLine1, BillingAddressLine1),
                new ChurnZeroAttribute(AccountExternalId, StandardAccountFields.BillingAddressLine2, BillingAddressLine2),
                new ChurnZeroAttribute(AccountExternalId, StandardAccountFields.BillingAddressCity, BillingAddressCity),
                new ChurnZeroAttribute(AccountExternalId, StandardAccountFields.BillingAddressState, BillingAddressState),
                new ChurnZeroAttribute(AccountExternalId, StandardAccountFields.BillingAddressZip, BillingAddressZip),
                new ChurnZeroAttribute(AccountExternalId, StandardAccountFields.BillingAddressCountry, BillingAddressCountry),
                new ChurnZeroAttribute(AccountExternalId, StandardAccountFields.StartDate, StartDate),
                new ChurnZeroAttribute(AccountExternalId, StandardAccountFields.EndDate, EndDate),
                new ChurnZeroAttribute(AccountExternalId, StandardAccountFields.LicenseCount, LicenseCount?.ToString()),
                new ChurnZeroAttribute(AccountExternalId, StandardAccountFields.OwnerUserAccount, OwnerUserAccount),
                new ChurnZeroAttribute(AccountExternalId, StandardAccountFields.ParentAccountExternalId, ParentAccountExternalId)
            }.Union(CustomFields.Select(x => new ChurnZeroAttribute(prefixCustomFields ? ChurnZeroCustomField.FormatDisplayNameToCustomFieldName(x.Key) : x.Key, x.Value, EntityTypes.Account, AccountExternalId))).Where(x => !string.IsNullOrWhiteSpace(x.Value));
        }
    }
}
