using ChurnZero.Sdk.Constants;
using ChurnZero.Sdk.Models;

namespace ChurnZero.Sdk.Requests
{
    internal class TrackEventRequest : ChurnZeroEventModel, IChurnZeroHttpRequest
    {
        public string AppKey { get; set; }
        public string Action => ChurnZeroActions.TrackEvent;
        //[JsonIgnore]
        //public override Dictionary<string, string> CustomFields { get; set; }
    }
}
