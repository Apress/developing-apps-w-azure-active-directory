using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MultitenantAPI.DAL
{
    public partial class TenantDBContext : DbContext
    {
        private readonly DBConnectionFactory connectionFactory;

        public TenantDBContext(DBConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public TenantDBContext(DbContextOptions<TenantDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Employees> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(this.connectionFactory.GetDbConnection());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employees>(entity =>
            {
                entity.HasKey(e => e.EmployeeId);

                entity.Property(e => e.Department).HasMaxLength(50);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SecondName).HasMaxLength(50);
            });
        }

        public async Task<List<Employees>> GetEmployees()
        {
            return await this.Employees.ToListAsync<Employees>();
        }
    }
}
