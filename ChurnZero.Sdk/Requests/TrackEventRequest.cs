using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using ChurnZero.Sdk.Constants;
using ChurnZero.Sdk.Models;

namespace ChurnZero.Sdk.Requests
{
    internal class TrackEventRequest : ChurnZeroEventModel, IChurnZeroHttpRequest
    {
        public string AppKey { get; set; }
        public string Action => ChurnZeroActions.TrackEvent;
        [JsonConverter(typeof(IsoDateConverter))]
        public override DateTime? EventDate { get; set; }
    }
}
