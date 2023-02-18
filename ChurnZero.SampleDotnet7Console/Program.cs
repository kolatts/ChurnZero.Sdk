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

//Creates your customer's Account in Churn Zero.
var accountResponse = await client.SetAttributeAsync(new ChurnZeroAttributeModel()
{
    AccountExternalId = "Test Account ID",
    Entity = ChurnZeroEntities.Account,
    Name = "Name",
    Value = "Test Customer"
});
Console.WriteLine($"Received {accountResponse.StatusCode} creating account");

//Creates your customer Account's Contact in Churn Zero. Must have an Account created first.
var contactResponse = await client.SetAttributeAsync(new ChurnZeroAttributeModel()
{
    AccountExternalId = "Test Account ID",
    Entity = ChurnZeroEntities.Contact,
    Name = "Name",
    Value = "Test Customer Contact"
});
Console.WriteLine($"Received {contactResponse} creating contact");
