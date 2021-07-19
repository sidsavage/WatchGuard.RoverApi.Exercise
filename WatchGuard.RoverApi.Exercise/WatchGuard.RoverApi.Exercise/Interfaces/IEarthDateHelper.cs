using System.Threading.Tasks;
using WatchGuard.RoverApi.Exercise.Models;

namespace WatchGuard.RoverApi.Exercise.Interfaces
{
    public interface IEarthDateHelper
    {
        public Task<EarthDateResults> GetEarthDatesAsync();
    }
}
