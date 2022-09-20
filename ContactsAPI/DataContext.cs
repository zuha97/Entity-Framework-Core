global using Microsoft.EntityFrameworkCore;

namespace ContactsAPI
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Contacts> contact => Set<Contacts>();

    }
}
