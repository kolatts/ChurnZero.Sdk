using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ChurnZero.Sdk.Constants;
using ChurnZero.Sdk.Decorators;
using ChurnZero.Sdk.Models;

namespace ChurnZero.Sdk
{
    public static class IChurnZeroAccountExtensions
    {
        private static readonly List<Type> _supportedTypes = new List<Type>()
        {
            typeof(DateTime),
            typeof(DateTime?),
            typeof(int),
            typeof(int?),
            typeof(double?),
            typeof(double),
            typeof(decimal),
            typeof(decimal?),
            typeof(string)
        };
        public static IEnumerable<ChurnZeroAttribute> ToAttributes(this IEnumerable<IChurnZeroAccount> accounts)
        {
            var list = accounts.ToList();
            if (!list.Any()) return new List<ChurnZeroAttribute>();
            var first = list.First();
            var properties = first.GetType()
                .GetProperties()
                .Where(x => x.GetCustomAttributes<ChurnZeroAccountAttributeAttribute>().Any())
                .ToList();
            var unsupportedProperties = properties.Where(x => !_supportedTypes.Contains(x.PropertyType)).ToList();
            if (unsupportedProperties.Any())
            {
                throw new NotSupportedException(
                    $"The properties {unsupportedProperties.Select(x => x.PropertyType.Name).Aggregate((x, y) => x + "," + y)} are not supported.");
            }
            if (!properties.Any()) return new List<ChurnZeroAttribute>();
            var attributes = properties
                .ToDictionary(x => x.Name, x => x.GetCustomAttribute<ChurnZeroAccountAttributeAttribute>());
            var results = new List<ChurnZeroAttribute>();
            foreach (var account in list)
            {
                foreach (var property in properties)
                {
                    results.Add(new ChurnZeroAttribute(ChurnZeroCustomField.FormatDisplayNameToCustomFieldName(attributes[property.Name].DisplayName), ResolveValue(property, account), EntityTypes.Account, account.AccountExternalId));
                }
            }
            return results;
        }

        public static IEnumerable<ChurnZeroAccount> ToChurnZeroAccounts(this IEnumerable<IChurnZeroAccount> accounts)
        {
            var list = accounts.ToList();
            if (!list.Any()) return new List<ChurnZeroAccount>();
            var first = list.First();
            var properties = first.GetType()
                .GetProperties()
                .Where(x => x.GetCustomAttributes<ChurnZeroAccountAttributeAttribute>().Any())
                .ToList();
            var unsupportedProperties = properties.Where(x => !_supportedTypes.Contains(x.PropertyType)).ToList();
            if (unsupportedProperties.Any())
            {
                throw new NotSupportedException(
                    $"The properties {unsupportedProperties.Select(x => x.PropertyType.Name).Aggregate((x, y) => x + "," + y)} are not supported.");
            }
            if (!properties.Any()) return new List<ChurnZeroAccount>();
            var attributes = properties
                .ToDictionary(x => x.Name, x => x.GetCustomAttribute<ChurnZeroAccountAttributeAttribute>());
            var results = new List<ChurnZeroAccount>();
            foreach (var account in list)
            {
                var accountResult = new ChurnZeroAccount()
                {
                    AccountExternalId = account.AccountExternalId
                };
                foreach (var property in properties)
                {
                    accountResult.CustomFields.Add(attributes[property.Name].DisplayName, ResolveValue(property, account));
                }
                results.Add(accountResult);
            }
            return results;
        }

        private static string ResolveValue(PropertyInfo property, IChurnZeroAccount account)
        {
            var valueAsObject = property.GetValue(account);
            if (valueAsObject == null) return string.Empty;
            if (property.PropertyType == typeof(DateTime?) || property.PropertyType == typeof(DateTime))
                return (valueAsObject as DateTime?)?.ToString();
            return valueAsObject?.ToString() ?? string.Empty;
        }
    }
}
