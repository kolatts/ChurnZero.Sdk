# Churn Zero SDK

This library is __not officially supported by [Churn Zero](https://www.churnzero.com/)__.
Licensed under the [Apache 2.0 License](LICENSE) (free for commercial and personal use). Contributions are welcome via pull request.


## Purpose

The purpose of this library is to create an easy-to-use means for .NET developers to incorporate Churn Zero into applications.


| API | Functionality | Basic Support | Custom Field Support
|-|-|-|-|
| HTTP | Setting Account Attributes (single) |✅|✅
| HTTP | Setting Contact Attributes (single) |✅|✅
| HTTP | Setting Account Attributes (multiple) |✅ |✅
| HTTP | Setting Contact Attributes (multiple) |✅|✅
| HTTP | Tracking Events (single) | ✅ | ❌
| HTTP | Tracking Events (multiple) | ❌ | ❌
| HTTP | Increment Attribute for Account or Contact | ❌ | ❌
| HTTP | Time in App | ❌ | ❌
| HTTP/CSV | Batch Setting Account Attributes | ❌ |❌
| HTTP/CSV | Batch Setting Contact Attributes | ❌ |❌
| HTTP/CSV | Batch Events | ❌ |❌


## Getting Started - Setup

Without dependency injection:
```cs
var client = new ChurnZeroHttpApiClient(new HttpClient() { BaseAddress = "https://mychurnzerourl.com/"}, "myAppKey"});
```

With dependency injection:

TBD

## Getting Started - Usage

```cs
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


```