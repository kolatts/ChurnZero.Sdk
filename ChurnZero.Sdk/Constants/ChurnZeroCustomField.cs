using System.Text.RegularExpressions;

namespace ChurnZero.Sdk.Constants
{
    public static class ChurnZeroCustomField
    {
        public static string FormatDisplayNameToCustomFieldName(string displayName)
        {
            return "cf_" + Regex.Replace(displayName, @"[^0-9a-zA-Z]+", "");
        }
    
    }
}
