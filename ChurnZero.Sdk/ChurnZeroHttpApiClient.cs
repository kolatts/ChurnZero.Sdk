﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ChurnZero.Sdk.Models;
using ChurnZero.Sdk.Requests;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ChurnZero.Sdk
{
    /// <summary>
    /// A client that makes HTTP calls to Churn Zero's HTTP API (distinct from REST API) to <i>push data into Churn Zero</i>
    /// </summary>
    public interface IChurnZeroHttpApiClient
    {
        /// <summary>
        /// Updates accounts in Churn Zero synchronously. For more granular control (including setting fields to blank) see <see cref="SetAttributes"/>
        /// </summary>
        /// <param name="accounts"></param>
        /// <returns></returns>
        HttpResponseMessage UpdateAccounts(params ChurnZeroAccount[] accounts);
        /// <summary>
        /// Updates accounts in Churn Zero asynchronously. For more granular control (including setting fields to blank) see <see cref="SetAttributesAsync"/>
        /// </summary>
        /// <param name="accounts"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> UpdateAccountsAsync(params ChurnZeroAccount[] accounts);

        /// <summary>
        /// Updates contacts in Churn Zero synchronously. For more granular control (including setting fields to blank) see <see cref="SetAttributes"/>
        /// </summary>
        /// <param name="contacts"></param>
        /// <returns></returns>
        HttpResponseMessage UpdateContacts(params ChurnZeroContact[] contacts);
        /// <summary>
        /// Updates contacts in Churn Zero asynchronously. For more granular control (including setting fields to blank) see <see cref="SetAttributesAsync"/>
        /// </summary>
        /// <param name="contacts"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> UpdateContactsAsync(params ChurnZeroContact[] contacts);

        /// <summary>
        /// Sets the attributes on an account or contact in a synchronous call.
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        HttpResponseMessage SetAttributes(params ChurnZeroAttribute[] attributes);
        /// <summary>
        /// Sets the attributes on an account or contact asynchronously.
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> SetAttributesAsync(params ChurnZeroAttribute[] attributes);
        /// <summary>
        /// Supports numeric attributes only. Value of attribute is what the increment is.
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        HttpResponseMessage IncrementAttributes(params ChurnZeroAttribute[] attributes);

        /// <summary>
        /// Supports numeric attributes only. Value of attribute is what the increment is.
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> IncrementAttributesAsync(params ChurnZeroAttribute[] attributes);
        /// <summary>
        /// Sends events in a synchronous call. Events require a contact and account external ID.
        /// </summary>
        /// <param name="events"></param>
        /// <returns></returns>
        HttpResponseMessage TrackEvents(params ChurnZeroEvent[] events);
        /// <summary>
        /// Sends events asynchronously. Events require a contact and account external ID.
        /// </summary>
        /// <param name="events"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> TrackEventsAsync(params ChurnZeroEvent[] events);
        /// <summary>
        /// Tracks time in apps synchronously. Start and End Dates are required.
        /// </summary>
        /// <param name="timeInApps"></param>
        /// <returns></returns>
        HttpResponseMessage TrackTimeInApps(params ChurnZeroTimeInApp[] timeInApps);
        /// <summary>
        /// Tracks time in apps asynchronously. Start and End Dates are required.
        /// </summary>
        /// <param name="timeInApps"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> TrackTimeInAppsAsync(params ChurnZeroTimeInApp[] timeInApps);

    }
    /// <inheritdoc cref="IChurnZeroHttpApiClient"/>
    public class ChurnZeroHttpApiClient : IChurnZeroHttpApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _appKey;

        private readonly JsonSerializerSettings _jsonSerializerSettings;
        public ChurnZeroHttpApiClient(HttpClient httpClient, string appKey)
        {
            if (string.IsNullOrWhiteSpace(appKey))
                throw new ArgumentNullException(nameof(appKey), "An app key is required.");
            _httpClient = httpClient;
            _appKey = appKey;
            _jsonSerializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy(),
                },
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
            };
        }

     
        public HttpResponseMessage UpdateAccounts(params ChurnZeroAccount[] accounts) => UpdateAccountsAsync(accounts).GetAwaiter().GetResult();
        public async Task<HttpResponseMessage> UpdateAccountsAsync(params ChurnZeroAccount[] accounts)
        {
            var attributes = accounts.SelectMany(x => x.ToAttributes()).ToArray();
            return await SetAttributesAsync(attributes);
        }

        public HttpResponseMessage UpdateContacts(params ChurnZeroContact[] contacts) =>
            UpdateContactsAsync(contacts).GetAwaiter().GetResult();
        public async Task<HttpResponseMessage> UpdateContactsAsync(params ChurnZeroContact[] contacts)
        {
            var attributes = contacts.SelectMany(x => x.ToAttributes()).ToArray();
            return await SetAttributesAsync(attributes);
        }

        public HttpResponseMessage SetAttributes(params ChurnZeroAttribute[] attributes) => SetAttributesAsync(attributes).GetAwaiter().GetResult();

        public async Task<HttpResponseMessage> SetAttributesAsync(params ChurnZeroAttribute[] attributes)
        {
            var requests = attributes.Select(x => new SetAttributeRequest(x, _appKey)).ToList();
            var serialized = JsonConvert.SerializeObject(requests, Formatting.Indented, _jsonSerializerSettings);
            var requestContent =
                new StringContent(serialized, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("i", requestContent);
            return response;
        }
        public HttpResponseMessage IncrementAttributes(params ChurnZeroAttribute[] attributes) => IncrementAttributesAsync(attributes).GetAwaiter().GetResult();

        public async Task<HttpResponseMessage> IncrementAttributesAsync(params ChurnZeroAttribute[] attributes)
        {
            var requests = attributes.Select(x => new IncrementAttributeRequest(x, _appKey)).ToList();
            var serialized = JsonConvert.SerializeObject(requests, Formatting.Indented, _jsonSerializerSettings);
            var requestContent =
                new StringContent(serialized, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("i", requestContent);
            return response;
        }
        public HttpResponseMessage TrackEvents(params ChurnZeroEvent[] events) => TrackEventsAsync(events).GetAwaiter().GetResult();

        public async Task<HttpResponseMessage> TrackEventsAsync(params ChurnZeroEvent[] events)
        {
            var requests = events.Select(x=> new TrackEventRequest(x, _appKey)).ToList();
            var serialized = JsonConvert.SerializeObject(requests, Formatting.Indented, _jsonSerializerSettings);
            var requestContent = new StringContent(serialized, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("i", requestContent);
            return response;
        }
        public HttpResponseMessage TrackTimeInApps(params ChurnZeroTimeInApp[] timeInApps) => TrackTimeInAppsAsync(timeInApps).GetAwaiter().GetResult();
        public async Task<HttpResponseMessage> TrackTimeInAppsAsync(params ChurnZeroTimeInApp[] timeInApps)
        {
            var requests = timeInApps.Select(x => new TimeInAppRequest(x, _appKey)).ToList();
            var serialized = JsonConvert.SerializeObject(requests, Formatting.Indented, _jsonSerializerSettings);
            var requestContent = new StringContent(serialized, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("i", requestContent);
            return response;
        }

     
    }
}
