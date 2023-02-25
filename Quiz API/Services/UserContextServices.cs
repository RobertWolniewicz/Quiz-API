using System.Security.Claims;

namespace Quiz_API.Services
{
    public interface IUserContextServices
    {
        int? GetUserid { get; }
        ClaimsPrincipal User { get; }
    }

    public class UserContextServices : IUserContextServices
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextServices(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;

        public int? GetUserid =>
            User is null ? null : (int?)int.Parse(User.FindFirst(C => C.Type == ClaimTypes.NameIdentifier).Value);
    }
}
