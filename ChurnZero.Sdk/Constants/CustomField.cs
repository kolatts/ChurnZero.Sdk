using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ChurnZero.Sdk.Constants
{
    public static class CustomField
    {
        public static string FormatDisplayNameToCustomFieldName(string displayName)
        {
            return Regex.Replace(displayName, @"[^0-9a-zA-Z]+", "");
        }
    }
}
