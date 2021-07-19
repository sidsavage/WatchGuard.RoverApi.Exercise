using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
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
            //Read dates from file
            var earthDateResults = await _earthDateHelper.GetEarthDatesAsync();
            AuditHelper.Audit(earthDateResults.UnformattableDateEntries);

            //Get image data for given dates
            var apiUrlStrings = new List<string>();
            foreach (var earthDate in earthDateResults.FormattedEarthDates)
                apiUrlStrings.Add(MarsRoverImageApi.GenerateUrl(_options.Value.ApiKey, earthDate));

            var imageData = await _imageApi.GetImageDataAsync(_httpClient, apiUrlStrings);

            //Download images
            var imageUrls = imageData.Select(imageDatum => imageDatum.Img_Src).ToList();
            await _imageApi.DownloadImages(_httpClient, imageUrls);
        }
    }
}
