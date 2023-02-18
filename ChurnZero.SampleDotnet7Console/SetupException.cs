using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ChurnZero.SampleDotnet7Console
{
    [Serializable]
    public class SetupException : Exception
    {
        public SetupException(string message) : base(message) { }
        public SetupException(string message, Exception innerException) : base(message, innerException) { }
        protected SetupException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public static void ThrowIfNull(string configKeyName, string? configValue)
        {
            if (string.IsNullOrEmpty(configValue))
                throw new SetupException($"{configKeyName} not found.See README.md for this project.");
        }
    }
}
