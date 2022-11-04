using Core.Domain;
using Core.DomainServices.Repositories;
using Core.DomainServices.Services.Implementations;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Core.DomainServices.Tests
{
    public class MealboxServiceTest
    {
        [Fact]
        [Trait("CreateNewMealbox", "MealboxService")]
        public async void CreateNewMealbox_Should_Return_Mealbox()
        {
            // Arrange
            Mock<ICategoryRepository> categoryRepository = new();
            Mock<ICanteenRepository> canteenRepository = new();
            Mock<IMealboxRepository> mealboxRepository = new();

            var sut = new MealboxService(categoryRepository.Object, canteenRepository.Object, mealboxRepository.Object);

            SetCategoryMoqData(categoryRepository);
            SetCanteenMoqData(canteenRepository);

            // Act
            var result = await sut.CreateNewMealbox(
                "Test Doos", 
                DateTime.Now.AddDays(3), 
                DateTime.Now.AddDays(3), 
                (decimal) 10.00, 
                1, 
                1
            );

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Mealbox>(result);

            Assert.NotNull(result.Canteen);
            Assert.IsType<Canteen>(result.Canteen);

            Assert.NotNull(result.Category);
            Assert.IsType<Category>(result.Category);
        }

        [Fact]
        [Trait("CreateNewMealbox", "MealboxService")]
        public void CreateNewMealbox_With_PickupFrom_Before_PickupFrom_Should_Throw_InvalidOperationException()
        {
            // Arrange
            Mock<ICategoryRepository> categoryRepository = new();
            Mock<ICanteenRepository> canteenRepository = new();
            Mock<IMealboxRepository> mealboxRepository = new();

            var sut = new MealboxService(categoryRepository.Object, canteenRepository.Object, mealboxRepository.Object);

            SetCategoryMoqData(categoryRepository);
            SetCanteenMoqData(canteenRepository);

            // Act / Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await sut.CreateNewMealbox(
                "Test Doos",
                DateTime.Now.AddDays(3),
                DateTime.Now.AddDays(3),
                (decimal)10.00,
                1,
                1
            ));
        }

        [Fact]
        [Trait("CreateNewMealbox", "MealboxService")]
        public void CreateNewMealbox_With_PickupFrom_Less_Than_Two_Days_Away_Throw_InvalidOperationException()
        {
            // Arrange
            Mock<ICategoryRepository> categoryRepository = new();
            Mock<ICanteenRepository> canteenRepository = new();
            Mock<IMealboxRepository> mealboxRepository = new();

            var sut = new MealboxService(categoryRepository.Object, canteenRepository.Object, mealboxRepository.Object);

            SetCategoryMoqData(categoryRepository);
            SetCanteenMoqData(canteenRepository);

            // Act / Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await sut.CreateNewMealbox(
                "Test Doos",
                DateTime.Now.AddDays(1), // Pickup date is less then 2 days away
                DateTime.Now.AddDays(1),
                (decimal)10.00,
                1,
                1
            ));
        }

        [Fact]
        [Trait("CreateNewMealbox", "MealboxService")]
        public void CreateNewMealbox_With_PickupFromDate_NotEqualTo_PickupUntilDate_Should_Throw_InvalidOperationException()
        {
            // Arrange
            Mock<ICategoryRepository> categoryRepository = new();
            Mock<ICanteenRepository> canteenRepository = new();
            Mock<IMealboxRepository> mealboxRepository = new();

            var sut = new MealboxService(categoryRepository.Object, canteenRepository.Object, mealboxRepository.Object);

            SetCategoryMoqData(categoryRepository);
            SetCanteenMoqData(canteenRepository);

            // Act / Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await sut.CreateNewMealbox(
                "Test Doos",
                DateTime.Now.AddDays(3), // Pickupfrom (date) is not equal to pickupuntil (date)
                DateTime.Now.AddDays(4),
                (decimal)10.00,
                1,
                1
            ));
        }

        [Fact]
        [Trait("CreateModifiedMealbox", "MealboxService")]
        public async void CreateModifiedMealbox_Should_Return_Mealbox()
        {
            // Arrange
            Mock<ICategoryRepository> categoryRepository = new();
            Mock<ICanteenRepository> canteenRepository = new();
            Mock<IMealboxRepository> mealboxRepository = new();

            var sut = new MealboxService(categoryRepository.Object, canteenRepository.Object, mealboxRepository.Object);

            SetCategoryMoqData(categoryRepository);
            SetCanteenMoqData(canteenRepository);
            SetMealboxMoqData(mealboxRepository); // No reservation

            // Act
            var result = await sut.CreateModifiedMealbox(
                1,
                "Test Doos",
                DateTime.Now,
                DateTime.Now,
                (decimal)10.00,
                1,
                1
            );

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Mealbox>(result);

            Assert.NotNull(result.Canteen);
            Assert.IsType<Canteen>(result.Canteen);

            Assert.NotNull(result.Category);
            Assert.IsType<Category>(result.Category);
        }

        [Fact]
        [Trait("CreateModifiedMealbox", "MealboxService")]
        public void CreateModifiedMealbox_With_Reservation_Should_Return_Mealbox()
        {
            // Arrange
            Mock<ICategoryRepository> categoryRepository = new();
            Mock<ICanteenRepository> canteenRepository = new();
            Mock<IMealboxRepository> mealboxRepository = new();

            var sut = new MealboxService(categoryRepository.Object, canteenRepository.Object, mealboxRepository.Object);

            SetCategoryMoqData(categoryRepository);
            SetCanteenMoqData(canteenRepository);
            SetMealboxWithReservationMoqData(mealboxRepository); // With reservation

            // Act / Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await sut.CreateModifiedMealbox(
                1,
                "Test Doos",
                DateTime.Now.AddDays(3),
                DateTime.Now.AddDays(3),
                (decimal)10.00,
                1,
                1
            ));
        }

        [Fact]
        [Trait("IsAgeBound", "MealboxService")]
        public async void IsAgeBound_Returns_True_When_Mealbox_Contains_AgeBound_Product()
        {
            // Arrange
            Mock<ICategoryRepository> categoryRepository = new();
            Mock<ICanteenRepository> canteenRepository = new();
            Mock<IMealboxRepository> mealboxRepository = new();

            var sut = new MealboxService(categoryRepository.Object, canteenRepository.Object, mealboxRepository.Object);

            SetMealboxWithAgeBoundProductMoqData(mealboxRepository); // Mealbox with agebound product

            // Act
            bool result = await sut.IsAgeBound(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait("IsAgeBound", "MealboxService")]
        public async void IsAgeBound_Returns_False_When_Mealbox_Contains_No_AgeBound_Product()
        {
            // Arrange
            Mock<ICategoryRepository> categoryRepository = new();
            Mock<ICanteenRepository> canteenRepository = new();
            Mock<IMealboxRepository> mealboxRepository = new();

            var sut = new MealboxService(categoryRepository.Object, canteenRepository.Object, mealboxRepository.Object);

            SetMealboxMoqData(mealboxRepository); // Mealbox with agebound product

            // Act
            bool result = await sut.IsAgeBound(1);

            // Assert
            Assert.False(result);
        }

        [Fact]
        [Trait("IsReserved", "MealboxService")]
        public async void IsReserved_Returns_True_When_Mealbox_Is_Reserved()
        {
            // Arrange
            Mock<ICategoryRepository> categoryRepository = new();
            Mock<ICanteenRepository> canteenRepository = new();
            Mock<IMealboxRepository> mealboxRepository = new();

            var sut = new MealboxService(categoryRepository.Object, canteenRepository.Object, mealboxRepository.Object);

            SetMealboxWithReservationMoqData(mealboxRepository); // Reserved mealbox

            // Act
            bool result = await sut.IsReserved(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        [Trait("IsReserved", "MealboxService")]
        public async void IsReserved_Returns_False_When_Mealbox_Is_Not_Reserved()
        {
            // Arrange
            Mock<ICategoryRepository> categoryRepository = new();
            Mock<ICanteenRepository> canteenRepository = new();
            Mock<IMealboxRepository> mealboxRepository = new();

            var sut = new MealboxService(categoryRepository.Object, canteenRepository.Object, mealboxRepository.Object);

            SetMealboxMoqData(mealboxRepository); // Mealbox withoud reservation

            // Act
            bool result = await sut.IsReserved(1);

            // Assert
            Assert.False(result);
        }

        private void SetCategoryMoqData(Mock<ICategoryRepository> repository)
        {
            repository.Setup(cr => cr.GetSingle(1)).ReturnsAsync(new Category()
            {
                Id = 1,
                Name = "Testpasta",
                Image = Array.Empty<byte>(),
            });
        }

        private void SetMealboxWithReservationMoqData(Mock<IMealboxRepository> repository)
        {
            repository.Setup(mbr => mbr.GetSingle(1)).ReturnsAsync(new Mealbox()
            {
                Id = 1,
                Name = "Testpasta",
                PickupFrom = DateTime.Now.AddDays(3),
                PickupUntil = DateTime.Now.AddDays(3),
                Reservation = new Reservation() // Reservation is set
            });
        }

        private void SetMealboxWithAgeBoundProductMoqData(Mock<IMealboxRepository> repository)
        {
            repository.Setup(mbr => mbr.GetSingle(1)).ReturnsAsync(new Mealbox()
            {
                Id = 1,
                Name = "Testpasta",
                PickupFrom = DateTime.Now.AddDays(3),
                PickupUntil = DateTime.Now.AddDays(3),
                Category = new()
                {
                    Id = 1,
                    Name = "Testpasta",
                    Image = Array.Empty<byte>(),
                    Products = new List<Product>()
                    {
                        new()
                        {
                            AgeBound = true, // Agebound product
                        }
                    }
                }
            });
        }

        private void SetMealboxMoqData(Mock<IMealboxRepository> repository)
        {
            repository.Setup(mbr => mbr.GetSingle(1)).ReturnsAsync(new Mealbox()
            {
                Id = 1,
                Name = "Testpasta",
                PickupFrom = DateTime.Now.AddDays(3),
                PickupUntil = DateTime.Now.AddDays(3),
                Category = new()
                {
                    Products = new List<Product>()
                }
            });
        }

        private void SetCanteenMoqData(Mock<ICanteenRepository> repository)
        {
            repository.Setup(cr => cr.GetSingle(1)).ReturnsAsync(new Canteen()
            {
                Id = 1,
                Name = "Het testcafe",
                City = City.BREDA,
            });
        }
    }
}
