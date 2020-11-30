namespace Infrastructure.Data
{
    public class DbInitializer : IDbInitializer
    {
        public void Initialize(ClinicContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
    }
}