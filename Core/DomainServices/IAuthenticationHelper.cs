using Core.Entities.Entities.BE;

namespace Core.Services.DomainServices
{
    public interface IAuthenticationHelper
    {
        string GenerateToken(Patient patient);
        string GenerateToken(Doctor doctor);
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt);
    }
}
