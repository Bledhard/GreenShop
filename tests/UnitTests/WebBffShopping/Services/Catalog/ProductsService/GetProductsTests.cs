﻿using Common.Models.Products;
using Common.Models.Specifications;
using FluentValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Bff.Shopping.Services.Catalog.Interfaces;
using Target = Web.Bff.Shopping.Services.Catalog.ProductsService;

namespace UnitTests.WebBffShopping.Services.Catalog.ProductsService
{
    [TestClass]
    public class GetProductsTests
    {
        private Mock<IConsumer<Product>> ProductsConsumerStub;
        private Target ProductsService;

        public GetProductsTests()
        {
            ProductsConsumerStub = new Mock<IConsumer<Product>>();
            ProductsService = new Target(ProductsConsumerStub.Object);
        }

        [TestMethod]
        public void ValidId_ReturnsExpectedType()
        {
            // Arrange
            int id = 1;

            ProductsConsumerStub
                .Setup(products => products.GetAsync(id))
                .Returns(Task.FromResult(ExpectedValidProduct));

            // Act
            Task<Product> result = ProductsService.GetProduct(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<Product>));
        }

        [TestMethod]
        public void ValidId_ReturnsExpectedProduct()
        {
            // Arrange
            int id = 1;

            ProductsConsumerStub
                .Setup(products => products.GetAsync(id))
                .Returns(Task.FromResult(ExpectedValidProduct));

            // Act
            Product result = ProductsService.GetProduct(id).Result;

            // Assert
            Assert.AreEqual(ExpectedValidProduct.Id, result.Id);
            Assert.AreEqual(ExpectedValidProduct.MongoId, result.MongoId);
            Assert.AreEqual(ExpectedValidProduct.Name, result.Name);
            Assert.AreEqual(ExpectedValidProduct.CategoryId, result.CategoryId);
            Assert.AreEqual(ExpectedValidProduct.Description, result.Description);
            Assert.AreEqual(ExpectedValidProduct.BasePrice, result.BasePrice);
            Assert.AreEqual(ExpectedValidProduct.Rating, result.Rating);
            Assert.AreEqual(ExpectedValidProduct.Specifications.First().Name, result.Specifications.First().Name);
            Assert.AreEqual(ExpectedValidProduct.Specifications.First().MaxSelectionAvailable, result.Specifications.First().MaxSelectionAvailable);
            Assert.AreEqual(ExpectedValidProduct.Specifications.First().Options.Count(), result.Specifications.First().Options.Count());
            Assert.AreEqual(ExpectedValidProduct.Specifications.First().Options.First(), result.Specifications.First().Options.First());
        }
        
        [TestMethod]
        public void InvalidId_ReturnsNull()
        {
            // Arrange
            int id = 99999;

            ProductsConsumerStub
                .Setup(products => products.GetAsync(id))
                .Returns(Task.FromResult(ExpectedInvalidProduct));

            // Act
            Product result = ProductsService.GetProduct(id).GetAwaiter().GetResult();

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void NegativeId_ThrowsValidationException()
        {
            // Arrange
            int id = -1;

            // Act
            Task<Product> result = ProductsService.GetProduct(id);

            // Assert
            Assert.AreEqual(result.Status, TaskStatus.Faulted);
            Assert.IsInstanceOfType(result.Exception.InnerException, typeof(ValidationException));
        }

        private Product ExpectedValidProduct
        {
            get
            {
                int id = 1;
                string name = "TestProduct";
                int parentId = 3;
                string description = "TestDescription";
                decimal basePrice = 12m;
                float rating = 4.5f;
                string specName = "sampleSpecification";
                int maxSelectionAvailable = 1;
                List<string> specOptions = new List<string> { "opt1" };

                Product product = new Product
                {
                    Id = id,
                    Name = name,
                    CategoryId = parentId,
                    Description = description,
                    BasePrice = basePrice,
                    Rating = rating,
                    Specifications = new List<Specification>
                    {
                        new Specification
                        {
                            Name = specName,
                            MaxSelectionAvailable = maxSelectionAvailable,
                            Options = specOptions
                        }
                    }
                };

                return product;
            }
        }

        private Product ExpectedInvalidProduct => null;
    }
}