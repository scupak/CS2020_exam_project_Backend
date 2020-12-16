namespace Infrastructure.Data
{
    public interface IDbInitializer
    {
        public void Initialize(ClinicContext context);
    }
}