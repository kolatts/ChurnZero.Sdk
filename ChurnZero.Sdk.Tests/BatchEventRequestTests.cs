using System.Globalization;
using ChurnZero.Sdk.Constants;
using ChurnZero.Sdk.Models;
using ChurnZero.Sdk.Requests;
using CsvHelper;
using CsvHelper.Configuration;

namespace ChurnZero.Sdk.Tests
{
    [TestClass]
    public class BatchEventRequestTests
    {
        [TestMethod]
        public void ToCsvOutput_Succeeds()
        {
            var request = new BatchEventRequest()
            {
                Events = new List<ChurnZeroBatchEvent>()
                {
                    new()
                    {
                        AccountExternalId = Guid.NewGuid().ToString(),
                        ContactExternalId = Guid.NewGuid().ToString(),
                        EventName = "Test Event Type",
                        EventDate = DateTime.Now,
                        Description = "Batch Import",
                        Quantity = 1,
                        CustomFields = new Dictionary<string, string>() {{"Test Account Custom Field 1", "1"}}
                    },
                    new()
                    {
                        AccountExternalId = Guid.NewGuid().ToString(),
                        ContactExternalId = Guid.NewGuid().ToString(),
                        EventName = "Test Event Type",
                        EventDate = DateTime.Now,
                        Description = "Batch Import",
                    },
                },
                AppKey = "test",
            };

            var output = request.ToCsvOutput();

            Assert.IsNotNull(output);
            var results = GetChurnZeroEvents(output);
            Assert.AreEqual(2, results.Count);
            Assert.AreEqual(request.Events[0].AccountExternalId, results[0].AccountExternalId);
            Assert.AreEqual(request.Events[0].ContactExternalId ?? string.Empty, results[0].ContactExternalId);
            Assert.AreEqual(request.Events[0].EventName ?? string.Empty, results[0].EventName);
            Assert.AreEqual(request.Events[0].EventDate, results[0].EventDate );
            Assert.AreEqual(request.Events[0].CustomFields["Test Account Custom Field 1"], results[0].CustomFields[ChurnZeroCustomField.FormatDisplayNameToCustomFieldName("Test Account Custom Field 1")]);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ComponentModel.DataAnnotations.ValidationException))]
        public void ToCsvOutput_ValidatesRequiredFields()
        {
            var request = new BatchEventRequest()
            {
                Events = new List<ChurnZeroBatchEvent>()
                {
                    new()
                    {
                        AccountExternalId = Guid.NewGuid().ToString(),
                        ContactExternalId = Guid.NewGuid().ToString(),
                        EventDate = DateTime.Now,
                        Description = "Batch Import",
                        Quantity = 1,
                        CustomFields = new Dictionary<string, string>() {{"Test Account Custom Field 1", "1"}}
                    },
                    new()
                    {
                        AccountExternalId = Guid.NewGuid().ToString(),
                        EventName = "Test Event Type",
                        EventDate = DateTime.Now,
                        Description = "Batch Import",
                    },
                },
                AppKey = "test",
            };
            request.ToCsvOutput();
        }

        [TestMethod]
        [ExpectedException(typeof(System.ComponentModel.DataAnnotations.ValidationException))]
        public void ToCsvOutput_Validates_ItemsIncluded()
        {
            var request = new BatchEventRequest()
            {
                Events = new List<ChurnZeroBatchEvent>() { },
                AppKey = "test",
            };
            request.ToCsvOutput();
        }

        private static List<ChurnZeroBatchEvent> GetChurnZeroEvents(string csvInput)
        {
            using var reader = new StringReader(csvInput);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture) { HeaderValidated = null, });
            csv.Read();
            csv.ReadHeader();
            var results = new List<ChurnZeroBatchEvent>();
            while (csv.Read())
            {
                var account = new ChurnZeroBatchEvent()
                {
                    AccountExternalId = csv.GetField<string>(nameof(ChurnZeroBatchEvent.AccountExternalId)),
                    ContactExternalId = csv.GetField<string>(nameof(ChurnZeroBatchEvent.ContactExternalId)),
                    EventName = csv.GetField<string>(nameof(ChurnZeroBatchEvent.EventName)),
                    EventDate = csv.GetField<DateTime?>(nameof(ChurnZeroBatchEvent.EventDate)),
                    Description = csv.GetField<string>(nameof(ChurnZeroBatchEvent.Description)),
                    Quantity = csv.GetField<int?>(nameof(ChurnZeroBatchEvent.Quantity)),
                    CustomFields = new Dictionary<string, string>() { { ChurnZeroCustomField.FormatDisplayNameToCustomFieldName("Test Account Custom Field 1"), csv.GetField<string>(ChurnZeroCustomField.FormatDisplayNameToCustomFieldName("Test Account Custom Field 1"))! } }
                };
                results.Add(account);
            }

            return results;
        }

    }
}