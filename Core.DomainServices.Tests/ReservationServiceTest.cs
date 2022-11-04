using Core.Domain;
using Core.DomainServices.Repositories;
using Core.DomainServices.Services.Implementations;
using Core.DomainServices.Services.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainServices.Tests
{
    public class ReservationServiceTest
    {
        [Fact]
        public async void CreateReservation_Should_Return_A_Reservation()
        {
            // Arrange
            Mock<IStudentRepository> studentRepository = new();
            Mock<IMealboxRepository> mealboxRepository = new();
            Mock<IMealboxService> mealboxService = new();

            var sut = new ReservationService(studentRepository.Object, mealboxRepository.Object, mealboxService.Object);

            SetMealboxAgeBoundData(mealboxService, mealboxRepository);
            SetStudentMoqData(studentRepository); // Student is old enough to buy agebound product

            // Act
            var result = await sut.CreateReservation(1, 1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Reservation>(result);

            Assert.NotNull(result.Student);
            Assert.IsType<Student>(result.Student);

            Assert.NotNull(result.Mealbox);
            Assert.IsType<Mealbox>(result.Mealbox);
        }

        [Fact]
        public void CreateReservation_With_Underage_Student_Should_Throw_A_ArgumentException()
        {
            // Arrange
            Mock<IStudentRepository> studentRepository = new();
            Mock<IMealboxRepository> mealboxRepository = new();
            Mock<IMealboxService> mealboxService = new();

            var sut = new ReservationService(studentRepository.Object, mealboxRepository.Object, mealboxService.Object);

            SetMealboxAgeBoundData(mealboxService, mealboxRepository);
            SetUnderagedStudentMoqData(studentRepository); // Student is not old enough

            // Act / Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await sut.CreateReservation(1, 1));
        }

        [Fact]
        public void CreateReservation_With_Duplicate_Box_PickupDate_Student_Should_Throw_A_ArgumentException()
        {
            // Arrange
            Mock<IStudentRepository> studentRepository = new();
            Mock<IMealboxRepository> mealboxRepository = new();
            Mock<IMealboxService> mealboxService = new();

            var sut = new ReservationService(studentRepository.Object, mealboxRepository.Object, mealboxService.Object);

            SetStudentWithReservationOnDateMoqData(studentRepository); // Student has a reservation today
            SetMealboxAgeBoundData(mealboxService, mealboxRepository);

            // Act / Assert
            Assert.ThrowsAsync < ArgumentException>(async () => await sut.CreateReservation(1, 1));
        }

        [Fact]
        public void CreateReservation_With_Reserved_Mealbox_Should_Throw_A_ArgumentException()
        {
            // Arrange
            Mock<IStudentRepository> studentRepository = new();
            Mock<IMealboxRepository> mealboxRepository = new();
            Mock<IMealboxService> mealboxService = new();

            var sut = new ReservationService(studentRepository.Object, mealboxRepository.Object, mealboxService.Object);

            SetStudentWithReservationOnDateMoqData(studentRepository); // Student has a reservation today
            SetMealboxAgeBoundData(mealboxService, mealboxRepository);

            // Act / Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await sut.CreateReservation(1, 1));
        }


        private void SetStudentMoqData(Mock<IStudentRepository> repository)
        {
            repository.Setup(sr => sr.GetSingle(1)).ReturnsAsync(new Student()
            {
                Id = 1,
                StudentNr = 420,
                Name = "Tesetpersoon",
                Birthdate = DateTime.Now.AddYears(-20), // Student is 18+
                Phonenumber = "+31 12345678",
                City = City.TILBURG,
                Reservations = new List<Reservation>()
            });
        }

        private void SetMealboxAgeBoundData(Mock<IMealboxService> service, Mock<IMealboxRepository> repository)
        {
            Mealbox mealbox = new Mealbox
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
                            AgeBound = true,
                        }
                    }
                }
            };

            repository.Setup(mbr => mbr.GetSingle(1)).ReturnsAsync(mealbox);
            service.Setup(mbs => mbs.IsReserved(1)).ReturnsAsync(mealbox.Reservation != null);
            service.Setup(mbs => mbs.IsAgeBound(1)).ReturnsAsync(mealbox.Category.Products.Any(p => p.AgeBound));
        }

        private void SetMealboxReservedData(Mock<IMealboxService> service)
        {
            Mealbox mealbox = new Mealbox
            {
                Id = 1,
                Name = "Testpasta",
                PickupFrom = DateTime.Now.AddDays(3),
                PickupUntil = DateTime.Now.AddDays(3),
                Reservation = new()
            };

            
            service.Setup(mbs => mbs.IsReserved(1)).ReturnsAsync(mealbox.Reservation != null);
        }

        private void SetStudentWithReservationOnDateMoqData(Mock<IStudentRepository> repository)
        {
            repository.Setup(sr => sr.GetSingle(1)).ReturnsAsync(new Student()
            {
                Id = 1,
                StudentNr = 420,
                Name = "Tesetpersoon",
                Birthdate = DateTime.Now.AddYears(-20), // Student is 18+
                Phonenumber = "+31 12345678",
                City = City.TILBURG,
                Reservations = new List<Reservation>()
                {
                    new()
                    {
                        Mealbox = new()
                        {
                            Id = 1,   
                            PickupFrom = DateTime.Now.AddDays(3),
                        }
                    }
                }
            });
        }

        private void SetUnderagedStudentMoqData(Mock<IStudentRepository> repository)
        {
            repository.Setup(sr => sr.GetSingle(1)).ReturnsAsync(new Student()
            {
                Id = 1,
                StudentNr = 420,
                Name = "Tesetpersoon",
                Birthdate = DateTime.Now.AddYears(-17), // Student is not 18+
                Phonenumber = "+31 12345678",
                City = City.TILBURG,
                Reservations = new List<Reservation>()
            });
        }
    }
}
