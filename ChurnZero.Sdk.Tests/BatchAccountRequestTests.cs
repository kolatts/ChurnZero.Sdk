using System.Globalization;
using ChurnZero.Sdk.Constants;
using ChurnZero.Sdk.Models;
using ChurnZero.Sdk.Requests;
using CsvHelper;
using CsvHelper.Configuration;

namespace ChurnZero.Sdk.Tests
{
    [TestClass]
    public class BatchAccountRequestTests
    {
        [TestMethod]
        public void BatchAccountRequest_ToCsvOutput_Succeeds()
        {
            var request = new BatchAccountRequest()
            {
                Accounts = new List<ChurnZeroAccount>()
                {
                    new()
                    {
                        AccountExternalId = Guid.NewGuid().ToString(),
                        IsActive = false,
                        BillingAddressCity = "Test1",
                        CustomFields = new Dictionary<string, string>() {{"Test Account Custom Field 1", "1"}}
                    },
                    new()
                    {
                        AccountExternalId = Guid.NewGuid().ToString(),
                        IsActive = true,
                        BillingAddressCity = "Test2",
                        CustomFields = new Dictionary<string, string>() {{"Test Account Custom Field 1", "2"}}
                    },
                },
                AppKey = "test",
            };

            var output = request.ToCsvOutput();

            Assert.IsNotNull(output);
            var results = GetChurnZeroAccounts(output);
            Assert.AreEqual(2, results.Count);
            Assert.AreEqual(request.Accounts[0].AccountExternalId, results[0].AccountExternalId);
            Assert.AreEqual(request.Accounts[0].IsActive, results[0].IsActive);
            Assert.AreEqual(request.Accounts[0].BillingAddressCity, results[0].BillingAddressCity);
            Assert.AreEqual(request.Accounts[0].CustomFields["Test Account Custom Field 1"], results[0].CustomFields[ChurnZeroCustomField.FormatDisplayNameToCustomFieldName("Test Account Custom Field 1")]);
        }
        [TestMethod]
        [ExpectedException(typeof(System.ComponentModel.DataAnnotations.ValidationException))]
        public void BatchAccountRequest_ToCsvOutput_ThrowsValidationException_IfCustomFieldsVaryByAccount()
        {
            var request = new BatchAccountRequest()
            {
                Accounts = new List<ChurnZeroAccount>()
                {
                    new()
                    {
                        AccountExternalId = Guid.NewGuid().ToString(),
                        IsActive = false,
                        BillingAddressCity = "Test1",
                        CustomFields = new Dictionary<string, string>() {{"Test Account Custom Field 1", "1"}}
                    },
                    new()
                    {
                        AccountExternalId = Guid.NewGuid().ToString(),
                        IsActive = true,
                        BillingAddressCity = "Test2",
                        CustomFields = new Dictionary<string, string>() {{"Test Account Custom Field 2", "2"}}
                    },
                },
                AppKey = "test",
            };
            request.ToCsvOutput();
        }
        private static List<ChurnZeroAccount> GetChurnZeroAccounts(string csvInput)
        {
            using var reader = new StringReader(csvInput);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture) { HeaderValidated = null, });
            csv.Read();
            csv.ReadHeader();
            var results = new List<ChurnZeroAccount>();
            while (csv.Read())
            {
                var account = new ChurnZeroAccount()
                {
                    AccountExternalId = csv.GetField<string>("accountExternalId"),
                    IsActive = csv.GetField<bool>(nameof(ChurnZeroAccount.IsActive)),
                    BillingAddressCity = csv.GetField<string>(nameof(ChurnZeroAccount.BillingAddressCity)),
                    CustomFields = new Dictionary<string, string>() { { ChurnZeroCustomField.FormatDisplayNameToCustomFieldName("Test Account Custom Field 1"), csv.GetField<string>(ChurnZeroCustomField.FormatDisplayNameToCustomFieldName("Test Account Custom Field 1"))! } }
                };
                results.Add(account);
            }

            return results;
        }

    }
}