using CoreArchitecture.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CoreArchitecture.Data.DBContext
{
    public class CoreArchitectureDBContext : DbContext
    {
        #region DBSets
        public DbSet<TestPerson> TestPerson { get; set; }
        public DbSet<TestDepartment> TestDepartment { get; set; }
   

        #endregion

        public CoreArchitectureDBContext(DbContextOptions<CoreArchitectureDBContext> options) : base(options)
        {
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
