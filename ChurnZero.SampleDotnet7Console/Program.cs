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
    new ChurnZeroAttributeModel(testAccountIdentifier, StandardAccountFields.Name, "Test Customer Account"),
    new ChurnZeroAttributeModel(testAccountIdentifier, StandardAccountFields.BillingAddressLine1, "123 Test Drive"),
    new ChurnZeroAttributeModel(testAccountIdentifier, StandardAccountFields.BillingAddressLine2, "Suite 3"),
    new ChurnZeroAttributeModel(testAccountIdentifier, StandardAccountFields.BillingAddressCity, "Testerville"),
    new ChurnZeroAttributeModel(testAccountIdentifier, StandardAccountFields.BillingAddressState, "Test"),
    new ChurnZeroAttributeModel(testAccountIdentifier, StandardAccountFields.StartDate, DateTime.Now)
);
Console.WriteLine($"Received {accountResponse.StatusCode} creating account");

//Creates a custom field in Churn Zero.
var testCustomFieldResponse = await client.SetAttributesAsync(new ChurnZeroAttributeModel("Test Custom Field", "Test Custom Field Value", EntityTypes.Account, testAccountIdentifier));
Console.WriteLine($"Received {testCustomFieldResponse.StatusCode} updating Custom Field on account");

//Creates your customer Account's Contact in Churn Zero. Must have an Account created first.
var contactResponse = await client.SetAttributesAsync(
    new ChurnZeroAttributeModel(testAccountIdentifier, testContactIdentifier, StandardContactFields.FirstName, "Joe"),
    new ChurnZeroAttributeModel(testAccountIdentifier, testContactIdentifier, StandardContactFields.LastName, "Tester")
    );
Console.WriteLine($"Received {contactResponse.StatusCode} creating contact");

//Creates events for a specific customer Account and Contact.
var eventResponse = await client.TrackEventsAsync(
    new ChurnZeroEventModel()
    {
        AccountExternalId = testAccountIdentifier, //Required
        ContactExternalId = testContactIdentifier, //Required
        Description = "Test Description", //Optional, can vary with event and is visible when viewing the individual events.
        EventName = "Test Event Type", //Required, this becomes the display name of the event in Churn Zero.
        EventDate = DateTime.Now, //Optional
        Quantity = 5, //Optional

    },
    new ChurnZeroEventModel()
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
