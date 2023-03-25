using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace WeatherStats.Data
{
    public static class CsvFileReader
    {
        public static List<T> ReadFile<T>(string filePath, int? startRowIndex = null, int? endRowIndex = null) where T : class
        {
            var itemType = typeof(T);

            var properties = itemType.GetProperties();

            var attributePropertyBundles = new Dictionary<CsvCollumnAttribute, PropertyInfo>();

            if (!properties.Any()) 
            {
                throw new FileLoadException();
            }

            foreach (var property in properties)
            {
                if (!property.CanWrite) continue;

                var attribute = property.GetCustomAttribute<CsvCollumnAttribute>();

                if (attribute != null)
                {
                    attributePropertyBundles[attribute] = property;
                }
            }

            var attributes = attributePropertyBundles.Keys;

            if (attributes.Count == 0)
                throw new FileLoadException();

            var lines = File.ReadAllLines(filePath);

            if (startRowIndex.HasValue && startRowIndex.Value < 0)
            {
                throw new ArgumentException(nameof(startRowIndex));
            }

            if (endRowIndex.HasValue && endRowIndex.Value < 0)
            {
                throw new ArgumentException(nameof(endRowIndex));
            }

            if (!startRowIndex.HasValue)
                startRowIndex = 0;

            if (!endRowIndex.HasValue)
                endRowIndex = lines.Length - 1;

            var items = new List<T>();

            for (var j = startRowIndex.Value; j <= endRowIndex.Value; j++)
            {
                var parts = lines[j].Split(';');

                if (parts.Length == 0)
                    throw new FileLoadException();

                var itemObj = Activator.CreateInstance(itemType);

                for (int i = 0; i < parts.Length; i++)
                {
                    var attribute = attributes.FirstOrDefault(a => a.Number == i);

                    if (attribute == null)
                        continue;

                    var property = attributePropertyBundles[attribute];

                    var converter = TypeDescriptor.GetConverter(property.PropertyType);

                    if(converter.CanConvertFrom(typeof(string)))
                    {
                        var value = converter.ConvertFrom(null, CultureInfo.InvariantCulture, parts[i]);
                        property.SetValue(itemObj, value);
                    }
                }

                items.Add(itemObj as T);
            }

            return items;
        }
    }

    public class CsvCollumnAttribute : Attribute
    {
        public int Number { get; set; }
    }
}
