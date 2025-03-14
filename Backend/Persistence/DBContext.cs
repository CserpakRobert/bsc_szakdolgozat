using Microsoft.EntityFrameworkCore;

namespace Backend.Persistence
{
    public partial class MariaDBContext: DbContext
    {
        public MariaDBContext(DbContextOptions<MariaDBContext> options) : base(options)
        {
           
        }
        public virtual DbSet<testDataModel> Test { get; set; }

    }
}
