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


//Creates your customer's Account in Churn Zero.
var accountResponse = await client.SetAttributeAsync(new ChurnZeroAttributeModel( StandardAccountFields.Name, testAccountIdentifier, "Test Customer Account"));
Console.WriteLine($"Received {accountResponse.StatusCode} creating account");

//Creates your customer's Account in Churn Zero.
var startDateResponse = await client.SetAttributeAsync(new ChurnZeroAttributeModel(StandardAccountFields.StartDate, testAccountIdentifier, DateTime.Now));
Console.WriteLine($"Received {startDateResponse.StatusCode} updating Start Date on account");

//Creates your customer Account's Contact in Churn Zero. Must have an Account created first.
var contactResponse = await client.SetAttributeAsync(new ChurnZeroAttributeModel(StandardContactFields.FirstName, testContactIdentifier, testAccountIdentifier, "Test Customer First Name"));
Console.WriteLine($"Received {contactResponse.StatusCode} creating contact");

//Creates events for a specific customer Account and Contact.
var eventResponse = await client.TrackEventAsync(new ChurnZeroEventModel()
{
    AccountExternalId = testAccountIdentifier,
    ContactExternalId = testContactIdentifier,
    Description = "Test Description",
    EventName = "Test Event Type",
    EventDate = DateTime.Now,
    Quantity = 5,
});
Console.WriteLine($"Received {eventResponse.StatusCode} tracking event");
