using System.Net;
using ChurnZero.Sdk;
using ChurnZero.Sdk.Constants;
using ChurnZero.Sdk.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>();
builder.Services.AddChurnZeroSdk((options) =>
{
    options.Url = builder.Configuration["ChurnZeroUrl"];
    options.AppKey = builder.Configuration["ChurnZeroAppKey"];
});
var app = builder.Build();

app.UseHttpsRedirection();
//See SampleDotnet7Console for more detailed examples; this sample exists for dependency injection demonstration.
app.MapGet("/testSetAttribute", async (IChurnZeroHttpApiClient client) =>
{
    await client.SetAttributesAsync(new ChurnZeroAttribute("Test Account ID", StandardAccountFields.Name,
        "Test Customer Account"));
    return Results.Ok("Test Account Created in Churn Zero");
});

app.Run();

