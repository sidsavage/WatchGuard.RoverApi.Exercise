using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WatchGuard.RoverApi.Exercise.Models;

namespace WatchGuard.RoverApi.Exercise.Actions
{
    public interface IImageApi
    {
        public Task<List<MarsRoverPhotoData>> GetImageDataAsync(HttpClient client, List<string> apiUrls);
    }
}
