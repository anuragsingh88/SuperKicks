using Microsoft.AspNetCore.Http;

namespace SuperKicks.Repo
{
    public static class TrackUser
    {
        private static IHttpContextAccessor? _httpContextAccessor;

        public static void Initialize(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static int AppUserID()
        {
            var appUserIdClaim = _httpContextAccessor?.HttpContext?.User?.FindFirst("AppUserID");
            if (appUserIdClaim != null && int.TryParse(appUserIdClaim.Value, out int appUserId))
            {
                return appUserId;
            }
            return 0;
        }

        public static T Created<T>(this T model) where T : class
        {
            var appUserId = AppUserID();
            var now = DateTimeOffset.Now;
            SetProperty(model, "CreatedBy", appUserId);
            SetProperty(model, "CreatedDateTime", now);
            return model;
        }

        public static T Updated<T>(this T model) where T : class
        {
            var appUserId = AppUserID();
            var now = DateTimeOffset.Now;
            SetProperty(model, "UpdatedBy", appUserId);
            SetProperty(model, "UpdatedDateTime", now);
            return model;
        }

        private static void SetProperty<T>(T model, string propertyName, object value)
        {
            var property = model?.GetType().GetProperty(propertyName);
            if (property != null && property.CanWrite)
            {
                property.SetValue(model, value);
            }
        }
    }
}
