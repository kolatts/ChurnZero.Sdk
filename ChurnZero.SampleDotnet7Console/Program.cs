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
const string testContactIdentifier = "Test Contact ID";

//Creates your customer's Account in Churn Zero or adjusts the name. CRM integration instead is recommended.
var accountResponse = await client.SetAttributesAsync(
    new ChurnZeroAttribute(testAccountIdentifier, StandardAccountFields.Name, "Test Customer Account"),
    new ChurnZeroAttribute(testAccountIdentifier, StandardAccountFields.BillingAddressLine1, "123 Test Drive"),
    new ChurnZeroAttribute(testAccountIdentifier, StandardAccountFields.BillingAddressLine2, "Suite 3"),
    new ChurnZeroAttribute(testAccountIdentifier, StandardAccountFields.BillingAddressCity, "Testerville"),
    new ChurnZeroAttribute(testAccountIdentifier, StandardAccountFields.BillingAddressState, "Test"),
    new ChurnZeroAttribute(testAccountIdentifier, StandardAccountFields.StartDate, DateTime.Now)
);
Console.WriteLine($"Received {accountResponse.StatusCode} creating account");

//Creates a custom field in Churn Zero.
var testCustomFieldResponse = await client.SetAttributesAsync(new ChurnZeroAttribute("Test Custom Field", "Test Custom Field Value", EntityTypes.Account, testAccountIdentifier));
Console.WriteLine($"Received {testCustomFieldResponse.StatusCode} updating Custom Field on account");

//Creates your customer Account's Contact in Churn Zero. Must have an Account created first.
var contactResponse = await client.SetAttributesAsync(
    new ChurnZeroAttribute(testAccountIdentifier, testContactIdentifier, StandardContactFields.FirstName, "Joe"),
    new ChurnZeroAttribute(testAccountIdentifier, testContactIdentifier, StandardContactFields.LastName, "Tester"),
    new ChurnZeroAttribute("Test Custom Field Value 2", 0.ToString(), EntityTypes.Contact, testAccountIdentifier, testContactIdentifier )
    );
Console.WriteLine($"Received {contactResponse.StatusCode} creating contact");

//Increments numeric attributes of accounts and contacts.
var incrementResponse = await client.IncrementAttributesAsync(
    new ChurnZeroAttribute(testAccountIdentifier, StandardAccountFields.LicenseCount, 1.ToString()),
    new ChurnZeroAttribute("Test Custom Field Value 2", 5.ToString(), EntityTypes.Contact, testAccountIdentifier,
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
