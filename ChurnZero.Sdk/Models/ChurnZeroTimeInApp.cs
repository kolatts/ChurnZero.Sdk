using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ChurnZero.Sdk.Models
{
    public class ChurnZeroTimeInApp
    {
        public ChurnZeroTimeInApp(string accountExternalId, string contactExternalId, DateTime startDate, DateTime endDate, string module = null)
        {
            AccountExternalId = accountExternalId;
            ContactExternalId = contactExternalId;
            StartDate = startDate;
            EndDate = endDate;
            Module = module;
        }
        [Required]
        public string AccountExternalId { get; set; }
        [Required]
        public string ContactExternalId { get; set; }
        /// <summary>
        /// Required.
        /// </summary>
        [Required]
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// Required.
        /// </summary>
        [Required]
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// Optional. When set, this will be reflected in the Churn Zero app.
        /// </summary>
        public string Module { get; set; }
    }
}
