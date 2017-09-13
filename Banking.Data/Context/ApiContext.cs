using Microsoft.EntityFrameworkCore;

namespace APITemplateCore.Data.Context
{
    public class ApiContext : DbContext
    {

        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
