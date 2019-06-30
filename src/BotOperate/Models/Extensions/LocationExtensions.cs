using BotOperate.Extensions;
using BotOperate.Models.DatabaseContext;

namespace BotOperate.Models.Extensions
{
    public static class LocationExtensions
    {
        public static string GetLocationString(this Location location)
        {
            if (location is null)
            {
                return string.Empty;
            }
            
            var locationText = location.City;
            if (location.State.HasValue())
            {
                locationText += $", {location.State}";
            }

            return locationText;
        }
    }
}