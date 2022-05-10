using Hair.Core.Models.System;
using Hair.Core.Models.User;
using Microsoft.EntityFrameworkCore;

namespace Hair.Repository.Context
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Page>? Pages { get; set; }
        public DbSet<User>? Users { get; set; }
        public DbSet<Role>? Roles { get; set; }
        public DbSet<UserRole>? UserRoles { get; set; }
        public DbSet<RoleMenu> RoleMenus { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //}
    }
}
