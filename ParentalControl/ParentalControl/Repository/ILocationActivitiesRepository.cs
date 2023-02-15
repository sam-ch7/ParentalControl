using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ParentalControl.Repository
{
    public interface ILocationActivitiesRepository
    {
        Task UploadLocation(Location location);
        Location GetLocation();
    }
}
