# Churn Zero SDK

This library is __not officially supported by [Churn Zero](https://www.churnzero.com/)__. 

Licensed under the [Apache 2.0 License](LICENSE) (free for commercial and personal use). Contributions are welcome via pull request.


## Purpose
The purpose of this library is to create an easy-to-use means for .NET developers to incorporate Churn Zero into applications.

*Note that until 1.x is formally released, all implementation is subject to change. Use at your own risk of refactoring.*

## Supported Direct Functionality
Legend: ✅Supported ❌Not supported currently ⭕ Not applicable

| API | Functionality | Basic Support | Custom Field Support
|-|-|-|-|
| HTTP | Setting Account Attributes (single) |✅|✅
| HTTP | Setting Contact Attributes (single) |✅|✅
| HTTP | Setting Account Attributes (multiple) |✅ |✅
| HTTP | Setting Contact Attributes (multiple) |✅|✅
| HTTP | Tracking Events (single) | ✅ | ❌
| HTTP | Tracking Events (multiple) | ✅ | ❌
| HTTP | Increment Attribute for Account or Contact | ✅ | ✅
| HTTP | Time in App | ✅ | ⭕
| HTTP/CSV | Batch Setting Account Attributes | ❌ |❌
| HTTP/CSV | Batch Setting Contact Attributes | ❌ |❌
| HTTP/CSV | Batch Events | ❌ |❌
| REST | Any | ❌ | ❌ |

## Supported Indirect Functionality
| Functionality | Supported | Comments
|-|-|-|
| Update accounts | ✅ | Wraps the standard and custom fields in a single account object
| Update contacts | ✅ | Wraps the standard and custom fields in a single contact object
| C# Attributes for decorating models | ❌ | Allows decorating a model class with attributes designating event or attribute


## Getting Started - Setup

Without dependency injection (see [sample project](ChurnZero.SampleDotnet7Console/Program.cs)):
```cs
var client = new ChurnZeroHttpApiClient(new HttpClient() { BaseAddress = "https://mychurnzerourl.com/"}, "myAppKey"});
```

With dependency injection:

```cs
services.AddHttpClient("ChurnZeroApiClient", client =>
{
    client.BaseAddress = new Uri("http://yourchurnzerourl");
});
services.AddScoped<IChurnZeroHttpApiClient, ChurnZeroHttpApiClient>(sp =>
{
    var httpClient = sp.GetRequiredService<IHttpClientFactory>()
                        .CreateClient("ChurnZeroApiClient");
    var appKey = configuration.GetValue<string>("ChurnZeroAppKey");
    return new ChurnZeroHttpApiClient(httpClient, appKey);
});

```

## Getting Started - Usage

All methods return HttpResponseMessages - successful or not - and the caller can decide whether or not to call `HttpResponseMessage.EnsureSuccessStatusCode()`.

```cs
//Usage
const string testAccountIdentifier = "Test Account ID";
const string testContactIdentifier = "Test Contact ID";

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


```