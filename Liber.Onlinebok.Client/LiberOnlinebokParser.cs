using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Liber.Onlinebok
{
    internal static class LiberOnlinebokParser
    {
        public static async Task<LiberOnlinebokDocument> ParseDocumentAsync(string json) =>
            await Task.Run(() => JsonConvert.DeserializeObject<LiberOnlinebokDocument>(json));

        public static async Task<Uri> ParseAssertsLocationAsync(string json) =>
            new Uri(await Task.Run(() => JObject.Parse(json).SelectToken("assetLocationResponse").Value<string>("assetLocationUrl")), UriKind.Absolute);

        public static async Task<LiberOnlinebokDocumentAttachment[]> ParseDocumentAttachmentsAsync(string json) =>
            await Task.Run(() => JObject.Parse(json).SelectToken("documentAttachments").ToObject<LiberOnlinebokDocumentAttachment[]>());
    }
}
