using System;
using System.Collections.Generic;
using System.Text;

namespace WatchGuard.RoverApi.Exercise.Api
{
    public static class MarsRoverApiSettings
    {
        public static string ApiUrl = "https://api.nasa.gov/mars-photos/api/v1/rovers/curiosity/photos?";

        public static string ApiKeyParam = "api_key={0}";

        public static string EarthDateParam = "earth_date={0}";

        public static string EarthDateFormat = "yyyy-MM-dd";

        public static string PageFormat = "page={0}";

        public static int PhotosPerPage = 25;

        public static int ChunkDownloadBy = 5;
    }
}
