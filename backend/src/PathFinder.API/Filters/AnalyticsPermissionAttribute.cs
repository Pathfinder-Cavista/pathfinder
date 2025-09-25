using Microsoft.AspNetCore.Mvc;

namespace PathFinder.API.Filters
{
    public class AnalyticsPermissionAttribute : TypeFilterAttribute
    {
        public AnalyticsPermissionAttribute() : 
            base(typeof(AnalyticsPermissionFilter)) { }
    }
}
