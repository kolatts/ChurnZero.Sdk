using ChurnZero.SampleDotnet7Console;
using ChurnZero.Sdk;
using ChurnZero.Sdk.Constants;
using ChurnZero.Sdk.Models;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder().AddUserSecrets<Program>();
var config = builder.Build();
SetupException.ThrowIfNull("Churn Zero URL", config["ChurnZeroUrl"]);
SetupException.ThrowIfNull("Churn Zero App Key", config["ChurnZeroAppKey"]);

var client = new ChurnZeroHttpApiClient(new HttpClient() { BaseAddress = new Uri(config["ChurnZeroUrl"]!) }, config["ChurnZeroAppKey"]);

//Usage
const string testAccountIdentifier = "Test Account ID";
const string testAccountIdentifier2 = "Test Account ID 2";
const string testContactIdentifier = "Test Contact ID";
const string testContactIdentifier2 = "Test Contact ID 2";

//Creates your customer's Account in Churn Zero or adjusts fields based on values supplied. CRM integration instead is recommended.
var accountResponse = await client.UpdateAccountsAsync(
    new ChurnZeroAccount()
    {
        AccountExternalId = testAccountIdentifier,
        Name = "Test Customer Account",
        BillingAddressLine1 = "321 Test Drive",
        BillingAddressLine2 = "Suite 2",
        BillingAddressCity = "Testerland",
        BillingAddressState = "Test",
        StartDate = DateTime.Now,
        CustomFields = new Dictionary<string,string>() { {"Test Custom Field", "Test Custom Field Value 1" } }
    }
);
Console.WriteLine($"Received {accountResponse.StatusCode} creating account");

//Performs the same add/update for Accounts in Churn Zero but scales much larger (500 MB file size).
//A 200 response may be returned, but since the accounts are processed separately from the API request, an email notification will indicate the success/failure of the import.
//Custom fields need to be added via non-batch methods first.
var accountBatchResponse = await client.UpdateAccountsBatchAsync(
    new List<ChurnZeroAccount>() {
        new ChurnZeroAccount()
    {
        AccountExternalId = testAccountIdentifier,
        Name = "Test Customer Account",
        BillingAddressLine1 = "321 Test Drive",
        BillingAddressLine2 = "Suite 2",
        BillingAddressCity = "Testerland",
        BillingAddressState = "Test",
        StartDate = DateTime.Now,
        CustomFields = new Dictionary<string, string>() { { "Test Custom Field", "Test Custom Field Value 1" } }
    },
    new ChurnZeroAccount()
    {
        AccountExternalId = testAccountIdentifier2,
        Name = "Test Customer Account 2",
        BillingAddressLine1 = "123 Test Drive",
        BillingAddressLine2 = "Suite 3",
        BillingAddressCity = "Testerville",
        BillingAddressState = "Test",
        StartDate = DateTime.Now,
        CustomFields = new Dictionary<string, string>() { { "Test Custom Field", "Test Custom Field Value 2" } }
    },
    }, "testAccountImport"
);
Console.WriteLine($"Received {accountBatchResponse.StatusCode} account batch add/update");


//Creates your customer Account's Contact in Churn Zero or adjusts fields based on values supplied. Must have an Account created first.
var contactResponse = await client.UpdateContactsAsync(new ChurnZeroContact()
{
    AccountExternalId = testAccountIdentifier,
    ContactExternalId = testContactIdentifier,
    FirstName = "Sunny",
    LastName = "Tester",
    Email = "test@test.com",
    CustomFields = new Dictionary<string, string>() {{"Test Custom Field Value 1", "0"}}
});
Console.WriteLine($"Received {contactResponse.StatusCode} creating contact");


//Performs the same add/update for Accounts in Churn Zero but scales much larger (500 MB file size).
//A 200 response may be returned, but since the contacts are processed separately from the API request, an email notification will indicate the success/failure of the import.
//Custom fields need to be added via non-batch methods first.
var contactBatchResponse = await client.UpdateContactsBatchAsync(new List<ChurnZeroContact>()
{
    new ChurnZeroContact()
    {
        AccountExternalId = testAccountIdentifier,
        ContactExternalId = testContactIdentifier,
        FirstName = "Sunny",
        LastName = "Tester",
        Email = "test@test.com",
        CustomFields = new Dictionary<string, string>() {{"Test Custom Field Value 1", "0"}}
    },
    new ChurnZeroContact()
    {
        AccountExternalId = testAccountIdentifier2,
        ContactExternalId = testContactIdentifier2,
        FirstName = "Joe",
        LastName = "Tester",
        Email = "test@test.com",
        CustomFields = new Dictionary<string, string>() {{"Test Custom Field Value 1", "2"}}
    }
}, "testContactImport");
Console.WriteLine($"Received {contactBatchResponse.StatusCode} contact batch add/update");

//Increments numeric attributes of accounts and contacts.
var incrementResponse = await client.IncrementAttributesAsync(
    new ChurnZeroAttribute(testAccountIdentifier, StandardAccountFields.LicenseCount, "1"),
    new ChurnZeroAttribute("Test Custom Field Value 2", "5", EntityTypes.Contact, testAccountIdentifier,
        testContactIdentifier));
Console.WriteLine($"Received {incrementResponse.StatusCode} incrementing attributes");

//Creates events for a specific customer Account and Contact.
var eventResponse = await client.TrackEventsAsync(
    new ChurnZeroEvent()
    {
        AccountExternalId = testAccountIdentifier, //Required
        ContactExternalId = testContactIdentifier, //Required
        Description = "Test Description", //Optional, can vary with event and is visible when viewing the individual events.
        EventName = "Test Event Type", //Required, this becomes the display name of the event in Churn Zero.
        EventDate = DateTime.Now, //Optional
        Quantity = 5, //Optional

    },
    new ChurnZeroEvent()
    {
        AccountExternalId = testAccountIdentifier, 
        ContactExternalId = testContactIdentifier, 
        Description = "Test Description", 
        EventName = "Test Event Type 2", 
        EventDate = DateTime.Now, 
        Quantity = 5, 
    }
    );
Console.WriteLine($"Received {eventResponse.StatusCode} tracking event");

//Creates time in app tracking for a specific Account and Contact
var timeInAppResponse = await client.TrackTimeInAppsAsync(
    new ChurnZeroTimeInApp(testAccountIdentifier, testContactIdentifier, DateTime.Now.AddHours(-1), DateTime.Now),
    new ChurnZeroTimeInApp(testAccountIdentifier, testContactIdentifier, DateTime.Now.AddHours(-1), DateTime.Now,
        "Test Module")
);
Console.WriteLine($"Received {timeInAppResponse.StatusCode} tracking time in app");
