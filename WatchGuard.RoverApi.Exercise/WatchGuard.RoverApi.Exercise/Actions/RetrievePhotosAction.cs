using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WatchGuard.RoverApi.Exercise.Api;
using WatchGuard.RoverApi.Exercise.Helpers;
using WatchGuard.RoverApi.Exercise.Interfaces;

namespace WatchGuard.RoverApi.Exercise.Actions
{
    public class RetrievePhotosAction
    {
        private readonly IEarthDateHelper _earthDateHelper;
        private readonly IImageApi _imageApi;
        private readonly IOptions<ApiOptions> _options;
        private readonly HttpClient _httpClient;
        public RetrievePhotosAction(IEarthDateHelper earthDateHelper,
            IImageApi imageApi,
            HttpClient httpClient,
            IOptions<ApiOptions> options)
        {
            _earthDateHelper = earthDateHelper;
            _imageApi = imageApi;
            _httpClient = httpClient;
            _options = options;
        }

        public async Task ExecuteAsync()
        {
            var earthDateResults = await _earthDateHelper.GetEarthDatesAsync();
            AuditHelper.Audit(earthDateResults.UnformattableDateEntries);

            var apiUrlStrings = new List<string>();
            foreach (var earthDate in earthDateResults.FormattedEarthDates)
                apiUrlStrings.Add(MarsRoverImageApi.GenerateUrl(_options.Value.ApiKey, earthDate));

            var imageData = await _imageApi.GetImageDataAsync(_httpClient, apiUrlStrings);
        }
    }
}
