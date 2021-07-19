using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WatchGuard.RoverApi.Exercise.Actions;
using WatchGuard.RoverApi.Exercise.Extensions;
using WatchGuard.RoverApi.Exercise.Helpers;
using WatchGuard.RoverApi.Exercise.Models;

namespace WatchGuard.RoverApi.Exercise.Api
{
    public class MarsRoverImageApi : IImageApi
    {
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly IOptions<ApiOptions> _dataOptions;

        public MarsRoverImageApi(IOptions<ApiOptions> options)
        {
            _dataOptions = options;
        }

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

        public async Task DownloadImages(HttpClient client, List<string> imagesToDownload)
        {
            Console.WriteLine($"Begin file download of { imagesToDownload.Count } image(s).");

            var imagesNotDownloaded = imagesToDownload.Where(imageUrl =>
            {
                var localFile = Path.Combine(Environment.CurrentDirectory, _dataOptions.Value.DataFolderName,
                                            Path.GetFileName(imageUrl));

                if (File.Exists(localFile))
                    return false;

                return true;
            })
            .ToList();

            if (imagesNotDownloaded.Count < imagesToDownload.Count)
                Console.WriteLine($"{imagesToDownload.Count - imagesNotDownloaded.Count} image(s) have already been downloaded");

            var chunkedImageStrings = imagesNotDownloaded.Chunk(MarsRoverApiSettings.ChunkDownloadBy).ToList();

            foreach (var chunkedImageString in chunkedImageStrings)
            {
                var downloadImageTasks = imagesNotDownloaded.Select(async (imageUrl) =>
                {
                    var localFilePath = Path.Combine(Environment.CurrentDirectory, _dataOptions.Value.DataFolderName,
                                                Path.GetFileName(imageUrl));

                    var photoBytes = await client.GetByteArrayAsync(imageUrl);

                    await File.WriteAllBytesAsync(localFilePath, photoBytes);
                });

                await Task.WhenAll(downloadImageTasks);
            }

            Console.WriteLine($"{imagesNotDownloaded.Count} image(s) have been downloaded");
        }
    }
}
