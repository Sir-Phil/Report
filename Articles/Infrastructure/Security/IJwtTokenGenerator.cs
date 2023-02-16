namespace Articles.Infrastructure.Security
{
    public interface IJwtTokenGenerator
    {
        string CreateToken(string Username);
    }
}
