using Core.Entities.Entities.BE;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data
{
    public interface IAuthenticationHelper
    {
        string GenerateToken(Patient patient);
        string GenerateToken(Doctor doctor);
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt);
    }
}
