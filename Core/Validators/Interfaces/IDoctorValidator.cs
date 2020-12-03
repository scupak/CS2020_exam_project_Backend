using Core.Entities.Entities.BE;

namespace Core.Services.Validators.Interfaces
{
    public interface IDoctorValidator
    {
        public void CreateValidation(Doctor doctor);
        public void EditValidation(Doctor doctor);
        public void IdValidation(int id);
    }
}