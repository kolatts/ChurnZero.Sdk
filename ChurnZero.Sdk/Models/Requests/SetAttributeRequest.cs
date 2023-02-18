using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;
using ChurnZero.Sdk.Constants;

namespace ChurnZero.Sdk.Models.Requests
{
    internal class SetAttributeRequest : ChurnZeroAttributeModel
    {
        public string AppKey { get; set; }
        public string Action => ChurnZeroActions.SetAttribute;
        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public override ChurnZeroEntities? Entity { get; set; }
    }
}
