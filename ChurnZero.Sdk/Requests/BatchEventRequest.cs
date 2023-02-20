using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using ChurnZero.Sdk.Constants;
using ChurnZero.Sdk.Models;
using CsvHelper;
using Newtonsoft.Json.Linq;

namespace ChurnZero.Sdk.Requests
{
    public class BatchEventRequest : IValidatableObject,  IChurnZeroHttpRequest
    {
        public string AppKey { get; set; }
        public string Action => ChurnZeroActions.BatchEvents;

        public List<ChurnZeroBatchEvent> Events { get; set; }

        public string ToCsvOutput()
        {
            Validator.ValidateObject(this, new ValidationContext(this));
            var allCustomFields = Events.SelectMany(x => x.CustomFields)
                .Select(x => x.Key)
                .Distinct()
                .ToList();
            string output;
            using (var writer = new StringWriter())
            {
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    var headerObject = new JObject()
                    {
                        {nameof(ChurnZeroBatchEvent.AccountExternalId), string.Empty},
                        {nameof(ChurnZeroBatchEvent.ContactExternalId), string.Empty},
                        {nameof(ChurnZeroBatchEvent.EventName), string.Empty},
                        {nameof(ChurnZeroBatchEvent.EventDate), string.Empty},
                        {nameof(ChurnZeroBatchEvent.Description), string.Empty},
                        {nameof(ChurnZeroBatchEvent.Quantity), string.Empty},
                    };
                    foreach (var customField in allCustomFields)
                    {
                        headerObject.Add(ChurnZeroCustomField.FormatDisplayNameToCustomFieldName(customField), string.Empty);
                    }
                    csv.WriteDynamicHeader(headerObject);
                    csv.NextRecord();
                    foreach (var e in Events)
                    {
                        csv.WriteField(e.AccountExternalId);
                        csv.WriteField(e.ContactExternalId);
                        csv.WriteField(e.EventName);
                        csv.WriteField(e.EventDate?.ToString("O"));
                        csv.WriteField(e.Description);
                        csv.WriteField(e.Quantity);
                        foreach (var customField in allCustomFields)
                        {
                            csv.WriteField(e.CustomFields.ContainsKey(customField) ? e.CustomFields[customField] : string.Empty);
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
            if (!Events.Any())
                yield return new ValidationResult("At least one event must be provided.");
            Events.ForEach(x=> Validator.ValidateObject(x, new ValidationContext(x)));
        }
    }
}
