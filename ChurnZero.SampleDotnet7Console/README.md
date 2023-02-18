# Setup

Retrieve the Churn Zero URL from your churn zero account, and the Application Key (AppKey)
Open a terminal in this project's directory and run the following commands:
```
dotnet user-secrets set ChurnZeroUrl <my-churn-zero-url>
dotnet user-secrets set ChurnZeroAppKey <my-churn-zero-appkey>
```
Example:
```
dotnet user-secrets set ChurnZeroUrl https://mycompany.us2app.churnzero.net/
dotnet user-secrets set ChurnZeroAppKey 9!SF6YwSCO-ZbltIkZgPJqkrhSHc-T1Ve3dvIBf3ibJNst22D3
```

## Important

A valid churn zero account is required.
The sample project will create test data, which will need to be removed by a request to Churn Zero.
Use of a development/test sandbox environment is encouraged.