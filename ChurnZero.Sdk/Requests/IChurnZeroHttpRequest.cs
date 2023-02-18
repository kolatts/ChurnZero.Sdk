using System;
using System.Collections.Generic;
using System.Text;

namespace ChurnZero.Sdk.Requests
{
    internal interface IChurnZeroHttpRequest
    {
        string AppKey { get; set; }
        string Action { get; }
    }
}
