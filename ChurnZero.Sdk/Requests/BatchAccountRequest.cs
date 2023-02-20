using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using ChurnZero.Sdk.Constants;
using ChurnZero.Sdk.Models;
using CsvHelper;
using Newtonsoft.Json.Linq;

namespace ChurnZero.Sdk.Requests
{
    public class BatchAccountRequest : IValidatableObject, IChurnZeroHttpRequest
    {
        public string AppKey { get; set; }
        public string Action => ChurnZeroActions.BatchAccounts;

        public List<ChurnZeroAccount> Accounts { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var customFields = Accounts.SelectMany(x => x.CustomFields.Keys).Distinct().ToList();
            if (Accounts.Select(x => x.CustomFields)
                .Any(x => !customFields.All(x.ContainsKey)))
            {
                yield return new ValidationResult("If custom fields are used, all accounts must contain them.");
            }
        }

        public string GetUrl(string notificationEmail = null)
        {
            var queryString = HttpUtility.ParseQueryString($"appKey={AppKey}");
            if (!string.IsNullOrWhiteSpace(notificationEmail))
            {
                queryString.Add("email", notificationEmail);
            }
            return $"{Action}?{queryString}";
        }

        public string ToCsvOutput()
        {
            Validator.ValidateObject(this, new ValidationContext(this));
            //It is possible that the first account does not have all the attributes defined, so we are trying to find the one with the most defined.
            var allAccountAttributes = Accounts.Select(x => x.ToAttributes(true)).ToList();
            var definedAccountAttributes =
                allAccountAttributes
                .OrderByDescending(x => x.Count())
                .First()
                .ToList();
            string output;
            using (var writer = new StringWriter())
            {
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    var headerObject = new JObject()
                    {
                        {"accountExternalId", definedAccountAttributes.First().AccountExternalId}
                    };
                    foreach (var attribute in definedAccountAttributes)
                    {
                        headerObject.Add(attribute.Name, attribute.Value);
                    }
                    csv.WriteDynamicHeader(headerObject);
                    csv.NextRecord();
                    foreach (var account in Accounts)
                    {
                        var accountAttributes = allAccountAttributes
                            .First(x =>
                                x.Any(y => y.AccountExternalId == account.AccountExternalId))
                            .ToDictionary(x => x.Name);
                        csv.WriteField(account.AccountExternalId);
                        foreach (var attributeName in definedAccountAttributes.Select(x => x.Name))
                        {
                            csv.WriteField(accountAttributes[attributeName].Value);
                        }
                        csv.NextRecord();
                    }

                    output = writer.ToString();
                }
            }
            return output;
        }


    }
}
