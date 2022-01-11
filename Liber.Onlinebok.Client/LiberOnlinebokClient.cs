﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Liber.Onlinebok
{
    public sealed class LiberOnlinebokClient : IDisposable
    {
        private readonly Guid _documentUuid;
        private readonly string _token;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a client using existing cookies and token.
        /// </summary>
        /// <param name="cookieContainer">Contains the required cookies for making authenticated requests.</param>
        /// <param name="token">E.g. "LIBER:userUUID=d8c0bc4d-1738-45b1-bec1-c0562927a81e&ticket=ST-m26DZjs95QTDEsrQQmM2VQidScFJt8yz9oZ2pNH5".</param>
        public LiberOnlinebokClient(Guid documentUuid, string token, CookieContainer cookieContainer)
        {
            _documentUuid = documentUuid;
            _token = token;

            HttpClientHandler handler = new()
            {
                AllowAutoRedirect = true,
                CookieContainer = cookieContainer,
                UseCookies = true
            };

            _httpClient = new(handler);

            _httpClient.DefaultRequestHeaders.Authorization = new("Token", _token);
        }

        public async Task<LiberOnlinebokDocument> GetDocumentAsync()
        {
            var url = $"https://onlinebokarkiv.liber.se/stendhal/api/v0/documents/{_documentUuid}";

            var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return await LiberOnlinebokParser.ParseDocumentAsync(json);
        }

        public async Task<Uri> GetAssetsLocationAsync()
        {
            var url = $"https://onlinebokarkiv.liber.se/stendhal/api/v0/assets/location/{_documentUuid}";

            var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return await LiberOnlinebokParser.ParseAssertsLocationAsync(json);
        }

        public async Task<LiberOnlinebokAssetsClient> GetAssetsClientAsync() => new LiberOnlinebokAssetsClient(await GetAssetsLocationAsync());

        public async Task<LiberOnlinebokDocumentAttachment[]> GetDocumentAttachments()
        {
            var url = $"https://onlinebokarkiv.liber.se/stendhal/api/v0/documents/{_documentUuid}/attachments";

            var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return await LiberOnlinebokParser.ParseDocumentAttachmentsAsync(json);
        }

        public void Dispose() => _httpClient.Dispose();

        public static LiberOnlinebokClient From(Uri uri, CookieContainer cookies)
        {
            var uriString = uri.ToString();

            var match = Regex.Match(uriString, "([0-9A-Fa-f]{8}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{12})");

            if (!match.Success)
                throw new FormatException("The format of the URL is invalid, no GUID found.");

            return new LiberOnlinebokClient(Guid.Parse(match.Value), uriString.SplitAndRemoveEmptyEntries('/').Last(), cookies);
        }
    }
}
