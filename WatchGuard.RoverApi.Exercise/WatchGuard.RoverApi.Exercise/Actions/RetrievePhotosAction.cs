using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WatchGuard.RoverApi.Exercise.Helpers;
using WatchGuard.RoverApi.Exercise.Interfaces;

namespace WatchGuard.RoverApi.Exercise.Actions
{
    public class RetrievePhotosAction
    {
        private readonly IEarthDateHelper _earthDateHelper;
        public RetrievePhotosAction(IEarthDateHelper earthDateHelper)
        {
            _earthDateHelper = earthDateHelper;
        }

        public async Task ExecuteAsync()
        {
            var earthDateResults = await _earthDateHelper.GetEarthDatesAsync();
        }
    }
}
