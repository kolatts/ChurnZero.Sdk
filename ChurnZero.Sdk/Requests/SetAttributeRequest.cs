using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;
using ChurnZero.Sdk.Constants;
using ChurnZero.Sdk.Models;

namespace ChurnZero.Sdk.Requests
{
    internal class SetAttributeRequest : ChurnZeroAttributeModel, IChurnZeroHttpRequest
    {

        public string AppKey { get; set; }
        public string Action => ChurnZeroActions.SetAttribute;
        [Required]
        [JsonIgnore]
        public override EntityTypes? EntityType { get; set; }

        public string Entity => EntityType.GetValueOrDefault().ToString().ToLower();
    }
}
