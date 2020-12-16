using Core.Entities.Entities.BE;

namespace Core.Services.Validators.Interfaces
{
    public interface IDoctorValidator
    {
        public void DefaultValidator(Doctor doctor);
        public void ValidateEmail(string email);

        public void ValidatePassword(string password);
    }
}