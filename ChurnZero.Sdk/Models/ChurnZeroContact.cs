using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ChurnZero.Sdk.Constants;

namespace ChurnZero.Sdk.Models
{
    public class ChurnZeroContact
    {
        [Required]
        public string AccountExternalId { get; set; }
        [Required]
        public string ContactExternalId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Dictionary<string, string> CustomFields { get; set; } = new Dictionary<string, string>();
        internal IEnumerable<ChurnZeroAttribute> ToAttributes(bool prefixCustomFields = false)
        {
            return new List<ChurnZeroAttribute>()
                {
                    new ChurnZeroAttribute(AccountExternalId, ContactExternalId, StandardContactFields.FirstName, FirstName),
                    new ChurnZeroAttribute(AccountExternalId, ContactExternalId, StandardContactFields.LastName, LastName),
                    new ChurnZeroAttribute(AccountExternalId, ContactExternalId, StandardContactFields.Email, Email),
                }
                .Union(CustomFields.Select(x => new ChurnZeroAttribute(prefixCustomFields ? ChurnZeroCustomField.FormatDisplayNameToCustomFieldName(x.Key) :x.Key, x.Value, EntityTypes.Contact, AccountExternalId, ContactExternalId)))
                .Where(x => !string.IsNullOrWhiteSpace(x.Value));
        }
    }
}