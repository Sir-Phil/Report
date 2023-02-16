using System.Security.Claims;

namespace Articles.Infrastructure.CurrentUser
{
    public class CurrentUserAccessor : ICurrentUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetCurrentUserName()
        {
            return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
        }
    }
}
