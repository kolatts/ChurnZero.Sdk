using ChurnZero.SampleDotnet7Console;
using ChurnZero.Sdk;
using ChurnZero.Sdk.Constants;
using ChurnZero.Sdk.Models;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder().AddUserSecrets<Program>();
var config = builder.Build();
SetupException.ThrowIfNull("Churn Zero URL", config["ChurnZeroUrl"]);
SetupException.ThrowIfNull("Churn Zero App Key", config["ChurnZeroAppKey"]);

var client = new ChurnZeroHttpApiClient(new HttpClient() { BaseAddress = new Uri(config["ChurnZeroUrl"]!)}, config["ChurnZeroAppKey"] );
 const string testAccountIdentifier = "Test Account ID";
const string testContactIdentifier = "Test Contact ID";


//Creates your customer's Account in Churn Zero or adjusts the name. CRM integration instead is recommended.
var accountResponse = await client.SetAttributeAsync(new ChurnZeroAttributeModel( testAccountIdentifier, StandardAccountFields.Name, "Test Customer Account"));
Console.WriteLine($"Received {accountResponse.StatusCode} creating account");

//Creates your customer's Account in Churn Zero. CRM integration instead is recommended.
var startDateResponse = await client.SetAttributeAsync(new ChurnZeroAttributeModel(testAccountIdentifier, StandardAccountFields.StartDate, DateTime.Now));
Console.WriteLine($"Received {startDateResponse.StatusCode} updating Start Date on account");

//Creates your customer Account's Contact in Churn Zero. Must have an Account created first.
var contactResponse = await client.SetAttributeAsync(new ChurnZeroAttributeModel(testAccountIdentifier, testContactIdentifier, StandardContactFields.FirstName, "Test Customer First Name"));
Console.WriteLine($"Received {contactResponse.StatusCode} creating contact");

//Creates events for a specific customer Account and Contact.
var eventResponse = await client.TrackEventAsync(new ChurnZeroEventModel()
{
    AccountExternalId = testAccountIdentifier, //Required
    ContactExternalId = testContactIdentifier, //Required
    Description = "Test Description", //Optional, can vary with event
    EventName = "Test Event Type", //Required
    EventDate = DateTime.Now, //Optional
    Quantity = 5, //Optional
});
Console.WriteLine($"Received {eventResponse.StatusCode} tracking event");
