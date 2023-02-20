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
    public class BatchAccountRequest :  IChurnZeroHttpRequest
    {
        public string AppKey { get; set; }
        public string Action => ChurnZeroActions.BatchAccounts;

        public List<ChurnZeroAccount> Accounts { get; set; }


        public string ToCsvOutput()
        {
            Validator.ValidateObject(this, new ValidationContext(this));
            var allAccountAttributes = Accounts.Select(x => x.ToAttributes(true)).ToList();
            var definedAccountAttributes =
                allAccountAttributes
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
                        {"accountExternalId", string.Empty}
                    };
                    foreach (var attribute in definedAccountAttributes)
                    {
                        headerObject.Add(attribute, string.Empty);
                    }
                    csv.WriteDynamicHeader(headerObject);
                    csv.NextRecord();
                    foreach (var account in Accounts.Select(x => x.AccountExternalId))
                    {
                        var accountAttributes = allAccountAttributes
                            .First(x =>
                                x.Any(y => y.AccountExternalId == account))
                            .ToDictionary(x => x.Name);
                        csv.WriteField(account);
                        foreach (var attributeName in definedAccountAttributes)
                        {
                            csv.WriteField(accountAttributes.ContainsKey(attributeName) ? accountAttributes[attributeName].Value : string.Empty);
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
