using System;
using System.Collections.Generic;
using System.Text;

namespace ChurnZero.Sdk.Decorators
{
    public abstract class BaseChurnZeroDecoratorAttribute : Attribute
    {
        public string DisplayName { get; set; }

        protected BaseChurnZeroDecoratorAttribute(string displayName)
        {
            DisplayName = displayName;
        }
    }
}
