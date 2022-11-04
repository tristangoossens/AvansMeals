using Core.Domain;
using Core.DomainServices.Repositories;
using Core.DomainServices.Services.Implementations;
using Core.DomainServices.Services.Interfaces;
using System;

namespace Core.DomainServices.Tests
{
    public class CategoryServiceTest
    {

        [Fact]
        [Trait("CreateNewCategory", "CategoryService")]
        public void CreateNewCategory_Should_Return_Category()
        {
            // Arrange
            Mock<ICategoryRepository> categoryRepository = new();
            Mock<IProductRepository> productRepository = new();
            var sut = new CategoryService(categoryRepository.Object, productRepository.Object);

            // Act
            var result = sut.CreateNewCategory("", Array.Empty<byte>());

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Category>(result);
        }

        [Fact]
        [Trait("CreateModifiedCategory", "CategoryService")]
        public async void CreateModifiedCategory_Should_Return_Category()
        {
            // Arrange
            Mock<ICategoryRepository> categoryRepository = new();
            Mock<IProductRepository> productRepository = new();

            var sut = new CategoryService(categoryRepository.Object, productRepository.Object);

            Category category = new Category
            {
                Id = 1,
                Name = "Testpasta",
                Image = Array.Empty<byte>(),
                Mealboxes = new List<Mealbox>
                {
                    // No reservation
                    new Mealbox(),
                }
            };
            categoryRepository.Setup(cr => cr.GetSingle(1)).ReturnsAsync(category);

            // Act
            var result = await sut.CreateModifiedCategory(category.Id, category.Name, category.Image);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Category>(result);
        }

        [Fact]
        [Trait("CreateModifiedCategory", "CategoryService")]
        public void CreateModifiedCategory_With_Reservation_Should_Throw_InvalidOperationException()
        {
            // Arrange
            Mock<ICategoryRepository> categoryRepository = new();
            Mock<IProductRepository> productRepository = new();
            var sut = new CategoryService(categoryRepository.Object, productRepository.Object);

            Category category = new()
            {
                Id = 1,
                Name = "Testpasta",
                Image = Array.Empty<byte>(),
                Mealboxes = new List<Mealbox>
                {
                    // Added reservation
                    new Mealbox {
                        Reservation = new Reservation {Id = 1}
                    },
                }
            };
            categoryRepository.Setup(cr => cr.GetSingle(1)).ReturnsAsync(category);

            // Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await sut.CreateModifiedCategory(category.Id, category.Name, category.Image));
        }

        [Fact]
        [Trait("CreateNewProductCategory", "CategoryService")]
        public async void CreateNewProductCategory_Should_Return_ProductCategory_Tuple()
        {
            // Arrange
            Mock<ICategoryRepository> categoryRepository = new();
            Mock<IProductRepository> productRepository = new();
            var sut = new CategoryService(categoryRepository.Object, productRepository.Object);

            Category category = new()
            {
                Id = 1,
                Name = "Testpasta",
                Image = Array.Empty<byte>(),
            };
            Product product = new()
            {
                Id = 1,
                AgeBound = false,
                Name = "Testspaghetti",
                Image = Array.Empty<byte>(),
            };

            productRepository.Setup(pr => pr.GetSingle(1)).ReturnsAsync(product);
            categoryRepository.Setup(cr => cr.GetSingle(1)).ReturnsAsync(category);

            // Act
            var result = await sut.CreateNewProductCategory(product.Id, category.Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Tuple<Product, Category>>(result);
        }

        [Fact]
        [Trait("CreateModifiedProductCategory", "CategoryService")]
        public async void CreateModifiedProductCategory_Should_Return_ProductCategory_Tuple()
        {
            // Arrange
            Mock<ICategoryRepository> categoryRepository = new();
            Mock<IProductRepository> productRepository = new();
            var sut = new CategoryService(categoryRepository.Object, productRepository.Object);

            Category category = new()
            {
                Id = 1,
                Name = "Testpasta",
                Image = Array.Empty<byte>(),
                Mealboxes = new List<Mealbox>
                {
                    // No reservation
                    new Mealbox()
                }
            };
            Product product = new()
            {
                Id = 1,
                AgeBound = false,
                Name = "Testspaghetti",
                Image = Array.Empty<byte>(),
            };

            productRepository.Setup(pr => pr.GetSingle(1)).ReturnsAsync(product);
            categoryRepository.Setup(cr => cr.GetSingle(1)).ReturnsAsync(category);

            // Act
            var result = await sut.CreateModifiedProductCategory(product.Id, category.Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Tuple<Product, Category>>(result);
        }

        [Fact]
        [Trait("CreateModifiedProductCategory", "CategoryService")]
        public void CreateModifiedProductCategory_With_Reservation_Should_Throw_InvalidOperationException()
        {
            // Arrange
            Mock<ICategoryRepository> categoryRepository = new();
            Mock<IProductRepository> productRepository = new();
            var sut = new CategoryService(categoryRepository.Object, productRepository.Object);

            Category category = new()
            {
                Id = 1,
                Name = "Testpasta",
                Image = Array.Empty<byte>(),
                Mealboxes = new List<Mealbox>
                {
                    // Added reservation
                    new Mealbox{
                        Reservation = new Reservation() 
                    }                
                }
            };

            Product product = new()
            {
                Id = 1,
                AgeBound = false,
                Name = "Testspaghetti",
                Image = Array.Empty<byte>(),
            };

            productRepository.Setup(pr => pr.GetSingle(1)).ReturnsAsync(product);
            categoryRepository.Setup(cr => cr.GetSingle(1)).ReturnsAsync(category);

            // Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await sut.CreateModifiedProductCategory(product.Id, category.Id));
        }

        [Fact]
        [Trait("IsAgeBound", "CategoryService")]
        public async void IsAgeBound_Returns_True_When_Category_Contains_AgeBound_Product()
        {
            // Arrange
            Mock<ICategoryRepository> categoryRepository = new();
            Mock<IProductRepository> productRepository = new();
            var sut = new CategoryService(categoryRepository.Object, productRepository.Object);

            Category category = new()
            {
                Id = 1,
                Name = "Testpasta",
                Image = Array.Empty<byte>(),
                Products = new List<Product>()
                {
                    new()
                    {
                        Id = 1,
                        AgeBound = true, // Leeftijdsgebonden
                        Name = "Test Wijntje",
                        Image = Array.Empty<byte>(),
                    }
                }
            };

            categoryRepository.Setup(cr => cr.GetSingle(1)).ReturnsAsync(category);

            // Act
            bool result = await sut.IsAgeBound(category.Id);

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait("IsAgeBound", "CategoryService")]
        public async void IsAgeBound_Returns_False_When_Category_Contains_No_AgeBound_Product()
        {
            // Arrange
            Mock<ICategoryRepository> categoryRepository = new();
            Mock<IProductRepository> productRepository = new();
            var sut = new CategoryService(categoryRepository.Object, productRepository.Object);

            Category category = new()
            {
                Id = 1,
                Name = "Testpasta",
                Image = Array.Empty<byte>(),
                Products = new List<Product>()
                {
                    new()
                    {
                        Id = 1,
                        AgeBound = false, // Niet leeftijdsgebonden
                        Name = "Testspaghetti",
                        Image = Array.Empty<byte>(),
                    }
                }
            };

            categoryRepository.Setup(cr => cr.GetSingle(1)).ReturnsAsync(category);

            // Act
            bool result = await sut.IsAgeBound(category.Id);

            // Assert
            Assert.False(result);
        }


        [Fact]
        [Trait("HasReservedPackage", "CategoryService")]
        public async void HasReservedPackage_Returns_True_When_Category_Contains_Reserved_Mealbox()
        {
            // Arrange
            Mock<ICategoryRepository> categoryRepository = new();
            Mock<IProductRepository> productRepository = new();
            var sut = new CategoryService(categoryRepository.Object, productRepository.Object);

            Category category = new()
            {
                Id = 1,
                Name = "Testpasta",
                Image = Array.Empty<byte>(),
                Mealboxes = new List<Mealbox>()
                {
                    new()
                    {
                        // Has a reservation
                        Reservation = new()
                    }
                }
            };

            categoryRepository.Setup(cr => cr.GetSingle(1)).ReturnsAsync(category);

            // Act
            bool result = await sut.HasReservedPackage(category.Id);

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait("HasReservedPackage", "CategoryService")]
        public async void HasReservedPackage_Returns_False_When_Category_Contains_No_Reserved_Mealbox()
        {
            // Arrange
            Mock<ICategoryRepository> categoryRepository = new();
            Mock<IProductRepository> productRepository = new();
            var sut = new CategoryService(categoryRepository.Object, productRepository.Object);

            Category category = new()
            {
                Id = 1,
                Name = "Testpasta",
                Image = Array.Empty<byte>(),
                Mealboxes = new List<Mealbox>()
                {
                    // Has no reservation
                    new()
                }
            };

            categoryRepository.Setup(cr => cr.GetSingle(1)).ReturnsAsync(category);

            // Act
            bool result = await sut.HasReservedPackage(category.Id);

            // Assert
            Assert.False(result);
        }
    }
}