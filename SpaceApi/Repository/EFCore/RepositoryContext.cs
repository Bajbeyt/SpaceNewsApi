using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Repository.EFCore;

public class RepositoryContext:IdentityDbContext<User,Roles,int>
{
    // TUser: Kullanıcıyı temsil eden sınıf türü.
    // TRole: Rolü temsil eden sınıf türü.
    // TKey: Kullanıcı ve rol anahtar türü.
    public RepositoryContext(DbContextOptions options) : base(options) //base options en temeldeki kalıtım alması için
    {
    }
    public DbSet<Rocket> Rockets { get; set; }
    public DbSet<News> News { get; set; }
    public DbSet<Axioms> Axioms { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder) //configrasyonlarımızı tutuyor
    {
        base.OnModelCreating(modelBuilder);
    }
}