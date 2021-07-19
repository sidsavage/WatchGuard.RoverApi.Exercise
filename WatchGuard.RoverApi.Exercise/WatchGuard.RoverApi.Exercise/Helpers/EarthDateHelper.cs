using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WatchGuard.RoverApi.Exercise.Api;
using WatchGuard.RoverApi.Exercise.Interfaces;
using WatchGuard.RoverApi.Exercise.Models;

namespace WatchGuard.RoverApi.Exercise.Helpers
{
    public class EarthDateHelper : IEarthDateHelper
    {
        private readonly IOptions<ApiOptions> _options;

        public EarthDateHelper(IOptions<ApiOptions> options)
        {
            _options = options;
        }
        public async Task<EarthDateResults> GetEarthDatesAsync()
        {
            var dataFilePath = Path.Combine(Environment.CurrentDirectory, _options.Value.DataFolderName, _options.Value.DataFileName);
            Console.WriteLine($"Reading earth dates to process from path {dataFilePath}");

            var dateStrings = string.Empty;
            using (StreamReader SourceReader = File.OpenText(dataFilePath))
                dateStrings = await SourceReader.ReadToEndAsync();

            var formattedDates = new List<string>();
            var unformattableDates = new List<string>();
            foreach (var dateString in dateStrings.Split(Environment.NewLine))
            {
                if (DateTime.TryParse(dateString, out var formattedDate))
                {
                    formattedDates.Add(formattedDate.ToString(MarsRoverApiSettings.EarthDateFormat));
                }
                else
                {
                    unformattableDates.Add(dateString);
                }
            }

            Console.WriteLine($"{formattedDates.Count} earth date(s) read and formatted.");

            return new EarthDateResults(formattedDates, unformattableDates);
        }
    }
}
