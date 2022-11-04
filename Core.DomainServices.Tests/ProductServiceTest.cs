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
    public class ProductServiceTest
    {
        [Fact]
        [Trait("CreateNewProduct", "ProductService")]
        public void CreateNewProduct_Should_Return_Product()
        {
            // Arrange
            Mock<ICategoryRepository> categoryRepository = new();
            var sut = new ProductService(categoryRepository.Object);

            // Act
            var result = sut.CreateNewProduct(
                "", 
                Array.Empty<byte>(), 
                false
            );

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Product>(result);
        }
    }
}
