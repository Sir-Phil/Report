namespace Articles.Infrastructure.Security
{
    public interface IPasswordHasher : IDisposable
    {
        Task<byte[]> Hash(string password, byte[] salt);
    }
}
