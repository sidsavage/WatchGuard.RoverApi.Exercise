using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WatchGuard.RoverApi.Exercise.Actions;
using WatchGuard.RoverApi.Exercise.Models;

namespace WatchGuard.RoverApi.Exercise.Api
{
    public class MarsRoverImageApi : IImageApi
    {
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public static string GenerateUrl(string apiKey, string date)
        {
            return string.Join('&', MarsRoverApiSettings.ApiUrl,
                string.Format(MarsRoverApiSettings.ApiKeyParam, apiKey),
                string.Format(MarsRoverApiSettings.EarthDateParam, date));
        }

        public async Task<List<MarsRoverPhotoData>> GetImageDataAsync(HttpClient client, List<string> apiUrls)
        {
            Console.WriteLine("Begin image data retrieval.");

            var results = new List<List<MarsRoverPhotoData>>();
            var getImageTasks = apiUrls.Select(async (apiUrl) =>
            {
                var photoData = new PhotoList();
                var photoList = new List<MarsRoverPhotoData>();
                var page = 0;
                do
                {
                    page++;
                    var pagedApiUrl = apiUrl + string.Format(MarsRoverApiSettings.PageFormat, page);

                    try
                    {
                        var photoJsonData = await client.GetStringAsync(pagedApiUrl);
                        photoData = JsonSerializer.Deserialize<PhotoList>(photoJsonData, _jsonOptions);
                        if (photoData != null && photoData.Photos.Any())
                            photoList.AddRange(photoData.Photos);
                    }
                    catch (Exception error)
                    {
                        throw new Exception($"Unabled to get resource specified at {pagedApiUrl} with exception {error.Message}");
                    }
                }
                while (photoData.Photos.Count == MarsRoverApiSettings.PhotosPerPage);

                return photoList;
            });

            results = (await Task.WhenAll(getImageTasks)).ToList();

            var distinctResults = results.SelectMany(x => x).Distinct().ToList();
            Console.WriteLine($"Retrieved {distinctResults.Count} image datum(s).");

            return distinctResults;
        }
    }
}
