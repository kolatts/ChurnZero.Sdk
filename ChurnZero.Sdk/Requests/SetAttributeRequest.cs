using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ChurnZero.Sdk.Constants;
using ChurnZero.Sdk.Models;
using Newtonsoft.Json;

namespace ChurnZero.Sdk.Requests
{
    internal class SetAttributeRequest :  IChurnZeroHttpRequest
    {
        public string AccountExternalId { get; set; }
        public virtual string ContactExternalId { get; set; }
        public string AppKey { get; set; }
        public string Action => ChurnZeroActions.SetAttribute;

        public string Entity => EntityType.GetValueOrDefault().ToString().ToLower();

        public string Name { get; set; }
        public string Value { get; set; }
        [JsonIgnore]
        public EntityTypes? EntityType { get; set; }
    }
}
