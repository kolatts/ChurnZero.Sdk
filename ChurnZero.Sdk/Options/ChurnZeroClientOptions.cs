namespace ChurnZero.Sdk.Options
{
    public class ChurnZeroClientOptions
    {
        /// <summary>
        /// The base URL / vanity URL for your organization in Churn Zero (e.g. https://mycompany.us2app.churnzero.net/)
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// The app key provided to you by Churn Zero
        /// </summary>
        public string AppKey { get; set; }
    }
}
