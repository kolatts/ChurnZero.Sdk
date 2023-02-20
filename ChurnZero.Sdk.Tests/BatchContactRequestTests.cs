using System.Globalization;
using ChurnZero.Sdk.Constants;
using ChurnZero.Sdk.Models;
using ChurnZero.Sdk.Requests;
using CsvHelper;
using CsvHelper.Configuration;

namespace ChurnZero.Sdk.Tests
{
    [TestClass]
    public class BatchContactRequestTests
    {
        [TestMethod]
        public void ToCsvOutput_Succeeds()
        {
            var request = new BatchContactRequest()
            {
                Contacts = new List<ChurnZeroContact>()
                {
                    new()
                    {
                        AccountExternalId = Guid.NewGuid().ToString(),
                        FirstName = "Sunny",
                        LastName = "Tester",
                        CustomFields = new Dictionary<string, string>() {{"Test Account Custom Field 1", "1"}}
                    },
                    new()
                    {
                        AccountExternalId = Guid.NewGuid().ToString(),
                        FirstName = "Joe",
                        Email = "Tester",
                        CustomFields = new Dictionary<string, string>() {{"Test Account Custom Field 1", "2"}}
                    },
                },
                AppKey = "test",
            };

            var output = request.ToCsvOutput();

            Assert.IsNotNull(output);
            var results = GetChurnZeroContacts(output);
            Assert.AreEqual(2, results.Count);
            Assert.AreEqual(request.Contacts[0].AccountExternalId, results[0].AccountExternalId);
            Assert.AreEqual(request.Contacts[0].FirstName ?? string.Empty, results[0].FirstName);
            Assert.AreEqual(request.Contacts[0].LastName ?? string.Empty, results[0].LastName);
            Assert.AreEqual(request.Contacts[0].Email ?? string.Empty, results[0].Email );
            Assert.AreEqual(request.Contacts[0].CustomFields["Test Account Custom Field 1"], results[0].CustomFields[ChurnZeroCustomField.FormatDisplayNameToCustomFieldName("Test Account Custom Field 1")]);
        }
   
        private static List<ChurnZeroContact> GetChurnZeroContacts(string csvInput)
        {
            using var reader = new StringReader(csvInput);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture) { HeaderValidated = null, });
            csv.Read();
            csv.ReadHeader();
            var results = new List<ChurnZeroContact>();
            while (csv.Read())
            {
                var account = new ChurnZeroContact()
                {
                    AccountExternalId = csv.GetField<string>("accountExternalId"),
                    FirstName = csv.GetField<string>(nameof(ChurnZeroContact.FirstName)),
                    LastName = csv.GetField<string>(nameof(ChurnZeroContact.LastName)),
                    Email = csv.GetField<string>(nameof(ChurnZeroContact.Email)),
                    CustomFields = new Dictionary<string, string>() { { ChurnZeroCustomField.FormatDisplayNameToCustomFieldName("Test Account Custom Field 1"), csv.GetField<string>(ChurnZeroCustomField.FormatDisplayNameToCustomFieldName("Test Account Custom Field 1"))! } }
                };
                results.Add(account);
            }

            return results;
        }

    }
}