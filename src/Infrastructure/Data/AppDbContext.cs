using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>().HasDiscriminator<UserType>("Type")
                .HasValue<User>(UserType.None)
                .HasValue<Client>(UserType.Client)
                .HasValue<Employee>(UserType.Employee)
                .HasValue<Owner>(UserType.Owner)
                .HasValue<SysAdmin>(UserType.SysAdmin);

            { //RELACIONES DEL APPOIMENT
                modelBuilder.Entity<Appointment>()
                    .HasOne<User>() 
                    .WithMany() 
                    .HasForeignKey(a => a.ProviderId)
                    .OnDelete(DeleteBehavior.Cascade);

                modelBuilder.Entity<Appointment>()
                    .HasOne<Service>() 
                    .WithMany() 
                    .HasForeignKey(a => a.ServiceId)
                    .OnDelete(DeleteBehavior.Cascade);


                modelBuilder.Entity<Appointment>()
                    .HasOne<Client>() 
                    .WithMany() 
                    .HasForeignKey(a => a.ClientId); 

                
                modelBuilder.Entity<Appointment>()
                    .HasOne<Shop>() 
                    .WithMany() 
                    .HasForeignKey(a => a.ShopId)
                    .OnDelete(DeleteBehavior.Cascade);
            }

            { //RELACIONES DEL EMPLOYEE
                modelBuilder.Entity<Employee>()
                    .HasOne<Shop>()
                    .WithMany()
                    .HasForeignKey(e => e.ShopId)
                    .OnDelete(DeleteBehavior.Cascade);
            }

            { //RELACIONES DEL OWNER
                modelBuilder.Entity<Owner>()
                    .HasOne<Shop>()
                    .WithMany()
                    .HasForeignKey(O => O.ShopId)
                    .OnDelete(DeleteBehavior.Cascade);
            }

            { //RELACIONES SERVICE
                modelBuilder.Entity<Service>()
                    .HasOne<Shop>()
                    .WithMany()
                    .HasForeignKey(s => s.ShopId)
                    .OnDelete(DeleteBehavior.Cascade);
            }


        }

    }
}
