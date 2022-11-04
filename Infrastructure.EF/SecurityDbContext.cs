using Core.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EF
{
    public class SecurityDbContext : IdentityDbContext
    {
        public SecurityDbContext(DbContextOptions<SecurityDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            SeedStudentData(modelBuilder);
            SeedEmployeeData(modelBuilder);
        }

        private void SeedStudentData(ModelBuilder modelBuilder)
        {
            string firstStudentGuid = Guid.NewGuid().ToString();
            string secondStudentGuid = Guid.NewGuid().ToString();

            List<IdentityUser> studentUsers = new()
            {
                new IdentityUser
                {
                    Id = firstStudentGuid,
                    Email = "tristan@avans.nl",
                    NormalizedEmail = "tristan@avans.nl".ToUpper(),
                    UserName = "Tristan",
                    NormalizedUserName = "Tristan".ToUpper(),
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D")
                },

                new IdentityUser
                {
                    Id = secondStudentGuid,
                    Email = "fleur@avans.nl",
                    NormalizedEmail = "fleur@avans.nl".ToUpper(),
                    UserName = "Fleur",
                    NormalizedUserName = "Fleur".ToUpper(),
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D")
                }
            };

            PasswordHasher<IdentityUser> hasher = new();
            studentUsers.ForEach(s => s.PasswordHash = hasher.HashPassword(s, "Test123!"));


            List<IdentityUserClaim<string>> userClaims = new()
            {
                new IdentityUserClaim<string>
                {
                    Id = 1,
                    UserId = firstStudentGuid,
                    ClaimType = "Student",
                    ClaimValue = "true"
                },

                new IdentityUserClaim<string>
                {
                    Id = 2,
                    UserId = secondStudentGuid,
                    ClaimType = "Student",
                    ClaimValue = "true"
                }
            };


            modelBuilder.Entity<IdentityUser>().HasData(studentUsers);
            modelBuilder.Entity<IdentityUserClaim<string>>().HasData(userClaims);
        }

        private void SeedEmployeeData(ModelBuilder modelBuilder)
        {
            string firstEmployeeGuid = Guid.NewGuid().ToString();

            List<IdentityUser> employeeUsers = new()
            {
                new IdentityUser
                {
                    Id = firstEmployeeGuid,
                    Email = "piet@avansmeals.nl",
                    NormalizedEmail = "piet@avansmeals.nl".ToUpper(),
                    UserName = "Piet",
                    NormalizedUserName = "Piet".ToUpper(),
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D")
                }
            };

            PasswordHasher<IdentityUser> hasher = new();
            employeeUsers.ForEach(s => s.PasswordHash = hasher.HashPassword(s, "Test123!"));


            List<IdentityUserClaim<string>> userClaims = new()
            {
                new IdentityUserClaim<string>
                {
                    Id = 3,
                    UserId = firstEmployeeGuid,
                    ClaimType = "KantineMedewerker",
                    ClaimValue = "true"
                },
            };


            modelBuilder.Entity<IdentityUser>().HasData(employeeUsers);
            modelBuilder.Entity<IdentityUserClaim<string>>().HasData(userClaims);
        }
    }
}
