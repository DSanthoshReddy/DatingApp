using API.Data;

namespace API.Services
{
    public interface ITokenService
    {
        string CreateToken(AppUser _user);
    }
}