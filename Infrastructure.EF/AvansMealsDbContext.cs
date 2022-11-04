using Core.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EF
{
    public class AvansMealsDbContext : DbContext
    {
        public DbSet<Canteen> Canteens { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Mealbox> Mealboxes { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Student> Students { get; set; }

        public AvansMealsDbContext(DbContextOptions<AvansMealsDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            SeedCanteenData(modelBuilder);
            SeedEmployeeData(modelBuilder);
            SeedStudentData(modelBuilder);

            SeedProductData(modelBuilder);
            SeedCategoryData(modelBuilder);

            SeedMealboxData(modelBuilder);
            SeedReservationData(modelBuilder);
        }

        private void SeedStudentData(ModelBuilder modelBuilder)
        {
            List<Student> students = new()
            {
                new Student
                {
                    Id = 1,
                    Name = "Tristan",
                    Email = "tristan@avans.nl",
                    City = City.BREDA,
                    Phonenumber = "+31 45875673",
                    Birthdate = DateTime.Parse("2002-04-02"),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },

                new Student
                {
                    Id = 2,
                    Name = "Fleur",
                    Email = "fleur@avans.nl",
                    City = City.BREDA,
                    Phonenumber = "+31 38992412",
                    Birthdate = DateTime.Parse("2005-08-12"),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },
            };

            // City enum conversion to string
            modelBuilder.Entity<Student>()
                .Property(s => s.City)
                .HasConversion<string>();

            // Insert list as table data
            modelBuilder.Entity<Student>().ToTable("Student").HasData(students);
        }

        private void SeedProductData(ModelBuilder modelBuilder)
        {
            var env = Environment.GetEnvironmentVariable("PATH_TO_SEED_IMAGES") ?? "../Infrastructure.EF/SeedImg";

            List<Product> products = new()
            {
                new Product
                {
                    Id = 1,
                    Name = "Paprika",
                    Image = File.ReadAllBytes(env + "/paprika.png"),
                    AgeBound = false,
                },

                new Product
                {
                    Id = 2,
                    Name = "Komkommer",
                    Image = File.ReadAllBytes(env + "/komkommer.png"),
                    AgeBound = false,
                },

                new Product
                {
                    Id = 3,
                    Name = "Saucijzenbrood",
                    Image = File.ReadAllBytes(env + "/saucijzenbroodje.png"),
                    AgeBound = false,
                },

                new Product
                {
                    Id = 4,
                    Name = "Keizerbolletje",
                    Image = File.ReadAllBytes(env + "/keizerbroodje.png"),
                    AgeBound = false,
                },

                new Product
                {
                    Id = 5,
                    Name = "Banaan",
                    Image = File.ReadAllBytes(env + "/banaan.jpg"),
                    AgeBound = false,
                },

                new Product
                {
                    Id = 6,
                    Name = "Sinaasappel",
                    Image = File.ReadAllBytes(env + "/sinaasappel.png"),
                    AgeBound = false,
                },

                new Product
                {
                    Id = 7,
                    Name = "Spahgetti",
                    Image = File.ReadAllBytes(env + "/spaghetti.jpg"),
                    AgeBound = false,
                },

                new Product
                {
                    Id = 8,
                    Name = "Penne",
                    Image = File.ReadAllBytes(env + "/penne.png"),
                    AgeBound = false,
                },

                new Product
                {
                    Id = 9,
                    Name = "Macaron",
                    Image = File.ReadAllBytes(env + "/macaron.jpg"),
                    AgeBound = false,
                },

                new Product
                {
                    Id = 10,
                    Name = "Tiramisu",
                    Image = File.ReadAllBytes(env + "/tiramisu.png"),
                    AgeBound = true,
                }
            };

            // Insert list as table data
            modelBuilder.Entity<Product>().ToTable("Product").HasData(products);
        }

        private void SeedCategoryData(ModelBuilder modelBuilder)
        {
            var env = Environment.GetEnvironmentVariable("PATH_TO_SEED_IMAGES") ?? "../Infrastructure.EF/SeedImg";

            List<Category> categories = new()
            {
                new Category
                {
                    Id = 1,
                    Name = "Brood",
                    Image = File.ReadAllBytes(env + "/brood.jpg"),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },

                new Category
                {
                    Id = 2,
                    Name = "Groenten",
                    Image = File.ReadAllBytes(env + "/groenten.jpg"),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },

                new Category
                {
                    Id = 3,
                    Name = "Fruit",
                    Image = File.ReadAllBytes(env + "/fruit.png"),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },

                new Category
                {
                    Id = 4,
                    Name = "Italiaans",
                    Image = File.ReadAllBytes(env + "/pasta.jpg"),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },

                new Category
                {
                    Id = 5,
                    Name = "Dessert",
                    Image = File.ReadAllBytes(env + "/dessert.jpg"),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                }
            };

            // Create intersection entity between category and product
            modelBuilder.Entity<Category>()
            .HasMany(c => c.Products)
            .WithMany(p => p.Categories)
            .UsingEntity<Dictionary<string, object>>(
                "ProductCategory",
                r => r.HasOne<Product>().WithMany().HasForeignKey("ProductId").OnDelete(DeleteBehavior.NoAction),
                l => l.HasOne<Category>().WithMany().HasForeignKey("CategoryId").OnDelete(DeleteBehavior.Cascade),
                ie =>
                {
                    ie.HasKey("ProductId", "CategoryId");
                    ie.HasData(
                        new { ProductId = 1, CategoryId = 2 },
                        new { ProductId = 2, CategoryId = 2 },
                        new { ProductId = 3, CategoryId = 1 },
                        new { ProductId = 4, CategoryId = 1 },
                        new { ProductId = 5, CategoryId = 3 },
                        new { ProductId = 6, CategoryId = 3 },
                        new { ProductId = 7, CategoryId = 4 },
                        new { ProductId = 8, CategoryId = 4 },
                        new { ProductId = 9, CategoryId = 5 },
                        new { ProductId = 10, CategoryId = 5 }
                    );
                });

            // Insert list as table data
            modelBuilder.Entity<Category>().ToTable("Category").HasData(categories);
        }

        private void SeedMealboxData(ModelBuilder modelBuilder)
        {
            List<Mealbox> mealboxes = new()
            {
                new Mealbox
                {
                    Id = 1,
                    Name = "Voorjaars fruitkist",
                    Price = Convert.ToDecimal(5.99),
                    PickupFrom = DateTime.Now.AddMonths(-9),
                    PickupUntil = DateTime.Now.AddMonths(-9).AddHours(3),
                    CanteenId = 2,
                    CategoryId = 3,
                    CreatedAt = DateTime.Now.AddMonths(-9).AddDays(-10),
                    UpdatedAt = DateTime.Now.AddMonths(-9).AddDays(-10),
                },

                new Mealbox
                {
                    Id = 2,
                    Name = "Broodassortiment",
                    Price = Convert.ToDecimal(5.99),
                    PickupFrom = DateTime.Now.AddMonths(-9),
                    PickupUntil = DateTime.Now.AddMonths(-9).AddHours(3),
                    CanteenId = 1,
                    CategoryId = 1,
                    CreatedAt = DateTime.Now.AddMonths(-9).AddDays(-10),
                    UpdatedAt = DateTime.Now.AddMonths(-9).AddDays(-10),
                },

                new Mealbox
                {
                    Id = 3,
                    Name = "Groentenkist",
                    Price = Convert.ToDecimal(4.99),
                    PickupFrom = DateTime.Now.AddDays(40).AddHours(5),
                    PickupUntil = DateTime.Now.AddDays(40).AddHours(5),
                    CanteenId = 1,
                    CategoryId = 2,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },

                new Mealbox
                {
                    Id = 4,
                    Name = "Warme broodjes",
                    Price = Convert.ToDecimal(7.49),
                    PickupFrom = DateTime.Now.AddDays(2),
                    PickupUntil = DateTime.Now.AddDays(2).AddHours(5),
                    CanteenId = 3,
                    CategoryId = 1,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },

                new Mealbox
                {
                    Id = 5,
                    Name = "Najaars fruitkist",
                    Price = Convert.ToDecimal(5.99),
                    PickupFrom = DateTime.Now.AddDays(10),
                    PickupUntil = DateTime.Now.AddDays(10).AddHours(5),
                    CanteenId = 2,
                    CategoryId = 3,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },

                new Mealbox
                {
                    Id = 6,
                    Name = "Toetjes",
                    Price = Convert.ToDecimal(10.00),
                    PickupFrom = DateTime.Now.AddDays(21).AddHours(5),
                    PickupUntil = DateTime.Now.AddDays(21).AddHours(5),
                    CanteenId = 3,
                    CategoryId = 5,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },

                new Mealbox
                {
                    Id = 7,
                    Name = "Pastakist",
                    Price = Convert.ToDecimal(8.50),
                    PickupFrom = DateTime.Now.AddDays(8).AddHours(5),
                    PickupUntil = DateTime.Now.AddDays(8).AddHours(5),
                    CanteenId = 4,
                    CategoryId = 4,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                }
            };

            // Set relation to reservation for navigation
            modelBuilder.Entity<Mealbox>()
                .HasOne(mb => mb.Reservation)
                .WithOne(r => r.Mealbox)
                .HasForeignKey<Reservation>(r => r.MealboxId);

            // Set price decimal precision
            modelBuilder.Entity<Mealbox>()
                .Property(mb => mb.Price)
                .HasColumnType("decimal(10,2)");

            // Delete mealbox on delete category
            modelBuilder
                .Entity<Mealbox>()
                .HasOne(mb => mb.Category)
                .WithMany(c => c.Mealboxes)
                .OnDelete(DeleteBehavior.ClientCascade);


            // Insert list as table data
            modelBuilder.Entity<Mealbox>().ToTable("Mealbox").HasData(mealboxes);
        }

        private void SeedEmployeeData(ModelBuilder modelBuilder)
        {
            List<Employee> employees = new()
            {
                new Employee
                {
                    Id = 1,
                    EmployeeNr = 21420,
                    Email = "piet@avansmeals.nl",
                    Name = "Piet",
                    CanteenId = 1,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                }
            };

            // Insert list as table data
            modelBuilder.Entity<Employee>().ToTable("Employee").HasData(employees);
        }

        private void SeedCanteenData(ModelBuilder modelBuilder)
        {
            List<Canteen> canteens = new()
            {
                new Canteen
                {
                    Id = 1,
                    City = City.BREDA,
                    Name = "Café the fifth",
                    WarmMeals = true,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },

                new Canteen
                {
                    Id = 2,
                    City = City.BREDA,
                    Name = "Kantine LD",
                    WarmMeals = false,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },

                new Canteen
                {
                    Id = 3,
                    City = City.TILBURG,
                    Name = "Café de vlugge hap",
                    WarmMeals = true,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },

                new Canteen
                {
                    Id = 4,
                    City = City.SHERTOGENBOSCH,
                    Name = "Kantine hoofdgebouw",
                    WarmMeals = true,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                }
            };

            // City enum conversion to string
            modelBuilder.Entity<Canteen>()
                .Property(c => c.City)
                .HasConversion<string>();

            // Insert list as table data
            modelBuilder.Entity<Canteen>().ToTable("Canteen").HasData(canteens);
        }

        private void SeedReservationData(ModelBuilder modelBuilder)
        {
            List<Reservation> reservations = new()
            {
                new Reservation {
                    Id = 1,
                    StudentId = 1,
                    MealboxId = 1,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },

                new Reservation {
                    Id = 2,
                    StudentId = 2,
                    MealboxId = 2,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                }
            };

            // Set foreign key relation to student
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Student)
                .WithMany(s => s.Reservations)
                .HasForeignKey(r => r.StudentId);

            // Insert list as table data
            modelBuilder.Entity<Reservation>().ToTable("Reservation").HasData(reservations);
        }
    }
}
