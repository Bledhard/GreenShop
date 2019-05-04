﻿using GreenShop.Web.Bff.Shopping.Api.Models.Categories;
using GreenShop.Web.Bff.Shopping.Api.Models.Comments;
using GreenShop.Web.Bff.Shopping.Api.Models.Products;
using GreenShop.Web.Bff.Shopping.Api.Services.Catalog.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using Target = GreenShop.Web.Bff.Shopping.Api.Services.CatalogService;

namespace GreenShop.Web.Bff.Shopping.UnitTests.Services.CatalogService
{
    [TestClass]
    public class GetCommentAsyncTests
    {
        private Mock<IConsumer<Category>> CategoriesConsumerStub;
        private Mock<IConsumer<Product>> ProductsConsumerStub;
        private Mock<ICommentsConsumer> CommentsConsumerStub;
        private Target CatalogService;

        public GetCommentAsyncTests()
        {
            CategoriesConsumerStub = new Mock<IConsumer<Category>>();
            ProductsConsumerStub = new Mock<IConsumer<Product>>();
            CommentsConsumerStub = new Mock<ICommentsConsumer>();
            CatalogService = new Target(CategoriesConsumerStub.Object, ProductsConsumerStub.Object, CommentsConsumerStub.Object);
        }

        [TestMethod]
        public void ReturnExpectedComment()
        {
            //Arrange
            int id = 1;
            CommentsConsumerStub
                .Setup(comments => comments.GetAsync(id))
                .Returns(Task.FromResult(ExpectedValidComment));

            //Act 
            Task<Comment> result = CatalogService.GetCommentAsync(id);
            Comment comment = result.GetAwaiter().GetResult();

            //Assert
            Assert.AreEqual(comment.Id, ExpectedValidComment.Id);
            Assert.AreEqual(comment.AuthorId, ExpectedValidComment.AuthorId);
            Assert.AreEqual(comment.Message, ExpectedValidComment.Message);
        }

        private Comment ExpectedValidComment
        {
            get
            {
                int id = 1;
                int authorId = 1;
                int parentID = 1;
                string message = "TestMessage";

                Comment comment = new Comment
                {
                    Id = id,
                    AuthorId = authorId,
                    ProductId = parentID,
                    Message = message
                };
                return comment;
            }
        }
    }
}