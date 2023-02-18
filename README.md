# Churn Zero SDK

This library is __not officially supported by [Churn Zero](https://www.churnzero.com/)__.
Licensed under the [Apache 2.0 License](LICENSE) (free for commercial and personal use). Contributions are welcome via pull request.


## Purpose

The purpose of this library is to create an easy-to-use means for .NET developers to incorporate Churn Zero into applications.


| API | Functionality | Supported in latest version|
|-|-|-|
| HTTP | Setting Account Attributes (single) |✅|
| HTTP | Setting Contact Attributes (single) |✅|
| HTTP | Setting Account Attributes (multiple) |❌|
| HTTP | Setting Contact Attributes (multiple) |❌|
| HTTP | Tracking Events (single) | ❌ |
| HTTP | Tracking Events (multiple) | ❌ |
| HTTP/CSV | Batch Setting Account Attributes | ❌ |
| HTTP/CSV | Batch Setting Contact Attributes | ❌ |
| HTTP/CSV | Batch Events | ❌ |


## Getting Started

Without dependency injection:
```cs
var client = new ChurnZeroHttpApiClient(new HttpClient() { BaseAddress = "https://mychurnzerourl.com/"}, "myAppKey"});
```

With dependency injection:

TBD