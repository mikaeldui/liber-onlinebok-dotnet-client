using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Liber.Onlinebok
{
    /// <summary>
    /// Deserializes JSON like "{ "something": {"id":"something", ... } }, "something2": ... }" to arrays, 
    /// that could otherwise have been deserialized to <see cref="Dictionary{string, TValue}"/>.
    /// </summary>
    /// <remarks>
    /// From https://stackoverflow.com/a/11953734/1275774
    /// </remarks>
    internal class DictionaryToArrayJsonConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (!objectType.IsArray) throw new ArgumentException("The property type must be an array.");
            if (reader.TokenType == JsonToken.Null) return null;

            var elementType = objectType.GetElementType();

            var jObject = JObject.Load(reader);
            var objects = jObject.Children().Select(j => j.First.ToObject(elementType)).ToArray();

            var array = Array.CreateInstance(elementType, objects.Count());

            objects.CopyTo(array, 0);

            return array;            
        }

        public override bool CanConvert(Type objectType) => false;

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => throw new NotImplementedException();
    }
}
