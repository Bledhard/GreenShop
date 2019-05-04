﻿using GreenShop.Web.Bff.Shopping.Api.Models.Categories;
using GreenShop.Web.Bff.Shopping.Api.Models.Comments;
using GreenShop.Web.Bff.Shopping.Api.Models.DTO;
using GreenShop.Web.Bff.Shopping.Api.Models.Products;
using GreenShop.Web.Bff.Shopping.Api.Services.Catalog.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenShop.Web.Bff.Shopping.Api.Controllers
{
    [ApiVersion("1")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogService _catalogService;

        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        #region Categories CRUD
        // GET api/catalog/categories
        [HttpGet("categories")]
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            IEnumerable<Category> categories = await _catalogService.GetAllCategoriesAsync();

            return categories;
        }

        // GET api/catalog/categories/5
        [HttpGet("categories/{id}")]
        public async Task<Category> GetCategoryAsync(int id)
        {
            Category category = await _catalogService.GetCategoryAsync(id);

            return category;
        }

        // POST api/catalog/categories
        [HttpPost("categories")]
        public async Task<int> AddCategoryAsync([FromBody] Category category)
        {
            int success = await _catalogService.AddCategoryAsync(category);

            return success;
        }

        // PUT api/catalog/categories/5
        [HttpPut("categories")]
        public async Task<bool> EditCategoryAsync([FromBody] Category category)
        {
            bool success = await _catalogService.EditCategoryAsync(category);

            return success;
        }

        // DELETE api/catalog/categories/5
        [HttpDelete("categories/{id}")]
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            bool success = await _catalogService.DeleteCategoryAsync(id);

            return success;
        }
        #endregion

        #region Products CRUD
        // GET api/catalog/products
        [HttpGet("products")]
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            IEnumerable<Product> products = await _catalogService.GetAllProductsAsync();

            return products;
        }

        // GET api/catalog/products/5
        [HttpGet("products/{id}")]
        public async Task<Product> GetProductAsync(int id)
        {
            Product product = await _catalogService.GetProductAsync(id);

            return product;
        }

        // POST api/catalog/products
        [HttpPost("products")]
        public async Task<int> AddProductAsync([FromBody] Product product)
        {
            int success = await _catalogService.AddProductAsync(product);

            return success;
        }

        // PUT api/catalog/products/5
        [HttpPut("products")]
        public async Task<bool> EditProductAsync([FromBody] Product product)
        {
            bool success = await _catalogService.EditProductAsync(product);

            return success;
        }

        // DELETE api/catalog/products/5
        [HttpDelete("products/{id}")]
        public async Task<bool> DeleteProductAsync(int id)
        {
            bool success = await _catalogService.DeleteProductAsync(id);

            return success;
        }
        #endregion

        #region Comments CRUD
        // GET api/catalog/comments/1
        [HttpGet("comments/{id}")]
        public async Task<Comment> GetComment(int id)
        {
            Comment result = await _catalogService.GetCommentAsync(id);

            return result;
        }

        // GET api/catalog/comments/products/2
        [HttpGet("comments/products/{id}")]
        public async Task<IEnumerable<Comment>> GetProductRelatedComment(int id)
        {
            IEnumerable<Comment> result = await _catalogService.GetAllProductCommentsAsync(id);

            return result;
        }

        // PUT api/catalog/comments/1
        [HttpPut("comments/{id}")]
        public async Task<bool> EditComment(int id, [FromBody] string message)
        {
            bool result = await _catalogService.EditCommentAsync(id, message);

            return result;
        }

        // PUT api/catalog/comments/1
        [HttpPut("comments/{id}")]
        public async Task<bool> EditComment(int id, [FromBody] Comment comment) => await EditComment(id, comment.Message);

        //Delete api/catalog/comments/1
        [HttpDelete("comments/{id}")]
        public async Task<bool> DeleteComment(int id)
        {
            bool result = await _catalogService.DeleteCommentAsync(id);

            return result;
        }

        //Post api/catalog/comments/1
        [HttpPost("comments")]
        public async Task<int> AddComment([FromBody] Comment comment)
        {
            int result = await _catalogService.AddCommentAsync(comment);

            return result;
        }
        #endregion

        // GET api/catalog/categories/5/products
        [HttpGet("categories/{id}/products")]
        public async Task<CategoryProductsDTO> GetCategoryWithRelatedProducts(int id)
        {
            CategoryProductsDTO result = await _catalogService.GetCategoryWithProductsAsync(id);

            return result;
        }

        // GET api/catalog/products/5/category
        [HttpGet("products/{id}/category")]
        public async Task<Product> GetProductWithCategoryAsync(int id)
        {
            Product product = await _catalogService.GetProductWithCategoryAsync(id);

            return product;
        }
    }
}