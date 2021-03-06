﻿using AutoMapper;
using GreenShop.Catalog.Api.Domain.Categories;
using GreenShop.Catalog.Api.Infrastructure;
using GreenShop.Catalog.Api.Service.Categories;
using GreenShop.Catalog.UnitTests.Wrappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Target = GreenShop.Catalog.Api.Service.Categories.CategoryService;

namespace GreenShop.Catalog.UnitTests.Service.Categories.CategoryService
{
    [TestClass]
    public class GetCategoryTests
    {
        private Mock<IDomainScope> UnitOfWorkStub;
        private Mock<IRepository<Category, CategoryDto>> CategoriesAccessorStub;
        private readonly Target Service;

        public GetCategoryTests()
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<Category, CategoryDto>());
            Mapper mapper = new Mapper(config);
            UnitOfWorkStub = new Mock<IDomainScope>();
            CategoriesAccessorStub = new Mock<IRepository<Category, CategoryDto>>();
            Service = new Target(mapper, UnitOfWorkStub.Object);
        }

        [TestInitialize]
        public void Setup()
        {
            UnitOfWorkStub.Setup(x => x.CategoryRepository)
                    .Returns(CategoriesAccessorStub.Object);
        }

        [TestMethod]
        public void ValidId_ReturnsExpectedType()
        {
            // Arrange
            int id = 1;

            CategoriesAccessorStub
                .Setup(categories => categories.GetAsync(id))
                .Returns(Task.FromResult(ExpectedValidCategory));

            // Act
            Task<CategoryDto> result = Service.GetAsync(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<CategoryDto>));
        }

        [TestMethod]
        public void ValidId_ReturnsExpectedCategory()
        {
            // Arrange
            int id = 1;

            CategoriesAccessorStub
                .Setup(categories => categories.GetAsync(id))
                .Returns(Task.FromResult(ExpectedValidCategory));

            // Act
            Task<CategoryDto> result = Service.GetAsync(id);
            CategoryDto actualCategory = result.GetAwaiter().GetResult();

            // Assert
            Assert.AreEqual(actualCategory.Id, ExpectedValidCategory.Id);
            Assert.AreEqual(actualCategory.Name, ExpectedValidCategory.Name);
            Assert.AreEqual(actualCategory.ParentCategoryId, ExpectedValidCategory.ParentCategoryId);
        }

        [TestMethod]
        public void InvalidId_ReturnsNull()
        {
            // Arrange
            int id = 99999;

            CategoriesAccessorStub
                .Setup(categories => categories.GetAsync(id))
                .Returns(Task.FromResult(ExpectedInvalidCategory));

            // Act
            Task<CategoryDto> result = Service.GetAsync(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<CategoryDto>));
            Assert.IsNull(result.Result);
        }

        [TestMethod]
        public void NegativeId_ReturnsNull()
        {
            // Arrange
            int id = -1;

            // Act
            Task<CategoryDto> result = Service.GetAsync(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Task<CategoryDto>));
            Assert.IsNull(result.Result);
        }

        private Category ExpectedValidCategory
        {
            get
            {
                int id = 1;
                string name = "TestCategory";
                int parentId = 2;

                Category category = new CategoryWrapper
                {
                    WrapId = id,
                    WrapName = name,
                    WrapParentCategoryId = parentId
                };

                return category;
            }
        }

        private Category ExpectedInvalidCategory
        {
            get
            {
                return null;
            }
        }
    }
}
