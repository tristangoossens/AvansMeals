using Core.Domain;
using Core.DomainServices.Repositories;
using Core.DomainServices.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainServices.Tests
{
    public class StudentServiceTest
    {
        [Fact]
        [Trait("CreateNewStudent", "StudentService")]
        public void CreateNewStudent_Should_Return_Category()
        {
            // Arrange
            var sut = new StudentService();

            // Act
            var result = sut.CreateNewStudent(
                21,
                "teststudent@avans.nl",
                "Test Student",
                DateTime.Now.AddYears(-20), // User is old enough
                "+31 12345678"
            );

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Student>(result);
        }


        [Fact]
        [Trait("CreateNewStudent", "StudentService")]
        public void CreateNewStudent_With_Birthdate_Under_16_Years_Should_Return_Category()
        {
            // Arrange
            var sut = new StudentService();

            // Act / Assert
            Assert.Throws<InvalidOperationException>(() => sut.CreateNewStudent(
                21,
                "teststudent@avans.nl",
                "Test Student",
                DateTime.Now.AddYears(-15), // User is not old enough
                "+31 12345678"
            ));
        }


        [Fact]
        [Trait("CreateNewStudent", "StudentService")]
        public void CreateNewStudent_With_Date_After_Should_Return_Category()
        {
            // Arrange
            var sut = new StudentService();

            // Act / Assert
            Assert.Throws<InvalidOperationException>(() => sut.CreateNewStudent(
                21,
                "teststudent@avans.nl",
                "Test Student",
                DateTime.Now.AddDays(5), // Date is after today
                "+31 12345678"
            ));
        }
    }
}
