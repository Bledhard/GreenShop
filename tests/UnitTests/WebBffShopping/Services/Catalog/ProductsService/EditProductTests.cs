﻿using Common.Models.Products;
using FluentValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Web.Bff.Shopping.Services.Catalog.Interfaces;
using Target = Web.Bff.Shopping.Services.Catalog.ProductsService;

namespace UnitTests.WebBffShopping.Services.Catalog.ProductsService
{
    [TestClass]
    public class EditProductTests
    {
        private Mock<IConsumer<Product>> ProductsConsumerStub;
        private Target ProductsService;

        public EditProductTests()
        {
            ProductsConsumerStub = new Mock<IConsumer<Product>>();
            ProductsService = new Target(ProductsConsumerStub.Object);
        }

        [TestMethod]
        public void ValidProduct_ReturnsTrue()
        {
            // Arrange
            int id = 1;
            string name = "RenamedTestProduct";
            int parentId = 3;
            string description = "TestDescription";
            decimal basePrice = 12m;
            float rating = 4.5f;
            bool expectedResult = true;

            Product product = new Product
            {
                Id = id,
                Name = name,
                CategoryId = parentId,
                Description = description,
                BasePrice = basePrice,
                Rating = rating
            };

            ProductsConsumerStub
                .Setup(products => products.EditAsync(product))
                .Returns(Task.FromResult(true));

            // Act
            Task<bool> result = ProductsService.EditProduct(product);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }

        [TestMethod]
        public void NegativeProductId_ThrowsValidationException()
        {
            // Arrange
            int id = -1;
            string name = "RenamedTestProduct";
            int parentId = 3;
            string description = "TestDescription";
            decimal basePrice = 12m;
            float rating = 4.5f;

            Product product = new Product
            {
                Id = id,
                Name = name,
                CategoryId = parentId,
                Description = description,
                BasePrice = basePrice,
                Rating = rating
            };

            // Act
            Task<bool> result = ProductsService.EditProduct(product);

            // Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        [TestMethod]
        public void InvalidProductId_ReturnsFalse()
        {
            // Arrange
            int id = 99999;
            string name = "NonExistingProduct";
            int parentId = 3;
            string description = "TestDescription";
            decimal basePrice = 12m;
            float rating = 4.5f;
            bool expectedResult = false;

            Product product = new Product
            {
                Id = id,
                Name = name,
                CategoryId = parentId,
                Description = description,
                BasePrice = basePrice,
                Rating = rating
            };

            ProductsConsumerStub
                .Setup(products => products.EditAsync(product))
                .Returns(Task.FromResult(false));

            // Act
            Task<bool> result = ProductsService.EditProduct(product);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<bool>));
            Assert.AreEqual(expectedResult, result.Result);
        }
    }
}
