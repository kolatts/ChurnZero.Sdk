using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using ChurnZero.Sdk.Constants;
using ChurnZero.Sdk.Models;
using CsvHelper;
using Newtonsoft.Json.Linq;
// ReSharper disable ConvertToUsingDeclaration

namespace ChurnZero.Sdk.Requests
{
    public class BatchContactRequest :  IValidatableObject, IChurnZeroHttpRequest
    {
        public string AppKey { get; set; }
        public string Action => ChurnZeroActions.BatchContacts;

        public List<ChurnZeroContact> Contacts { get; set; }
        
        public string ToCsvOutput()
        {
            Validator.ValidateObject(this, new ValidationContext(this));
            //It is possible that the first account does not have all the attributes defined, so we are getting a distinct list of all attribute names.
            var allContactAttributes = Contacts.Select(x => x.ToAttributes(true)).ToList();
            var definedContactAttributes =
                allContactAttributes
                    .SelectMany(x => x)
                    .Select(x => x.Name)
                    .Distinct()
                    .ToList();
            string output;
            using (var writer = new StringWriter())
            {
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    var headerObject = new JObject()
                    {
                        {"accountExternalId", string.Empty},
                        {"contactExternalId", string.Empty},
                    };
                    foreach (var attribute in definedContactAttributes)
                    {
                        headerObject.Add(attribute, string.Empty);
                    }
                    csv.WriteDynamicHeader(headerObject);
                    csv.NextRecord();
                    foreach (var contact in Contacts)
                    {
                        var contactAttributes = allContactAttributes
                            .First(x =>
                                x.Any(y => y.AccountExternalId == contact.AccountExternalId && y.ContactExternalId == contact.ContactExternalId))
                            .ToDictionary(x => x.Name);
                        csv.WriteField(contact.AccountExternalId);
                        csv.WriteField(contact.ContactExternalId);
                        foreach (var attributeName in definedContactAttributes)
                        {
                            csv.WriteField(contactAttributes.ContainsKey(attributeName) ? contactAttributes[attributeName].Value : string.Empty);
                        }
                        csv.NextRecord();
                    }

                    output = writer.ToString();
                }
            }
            return output;
        }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Contacts.Any())
                yield return new ValidationResult("At least one contact must be supplied.");
            Contacts.ForEach(x=> Validator.ValidateObject(x, new ValidationContext(x)));
        }
    }
}
