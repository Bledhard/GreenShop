﻿using GreenShop.Web.Bff.Shopping.Api.Models.Categories;
using GreenShop.Web.Bff.Shopping.Api.Models.Comments;
using GreenShop.Web.Bff.Shopping.Api.Models.DTO;
using GreenShop.Web.Bff.Shopping.Api.Models.Products;
using GreenShop.Web.Bff.Shopping.Api.Services.Catalog.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenShop.Web.Bff.Shopping.Api.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly IConsumer<Category> _categoriesConsumer;
        private readonly IConsumer<Product> _productsConsumer;
        private readonly ICommentsConsumer _commentsConsumer;

        public CatalogService(IConsumer<Category> categoriesConsumer,
                              IConsumer<Product> productsConsumer,
                              ICommentsConsumer commentsConsumer)
        {
            _categoriesConsumer = categoriesConsumer;
            _productsConsumer = productsConsumer;
            _commentsConsumer = commentsConsumer;
        }

        #region Categories
        /// <summary>
        /// Asynchronously adds Category
        /// </summary>
        /// <param name="category">Category to add</param>
        /// <returns>Task with Category id</returns>
        public async Task<int> AddCategoryAsync(Category category)
        {
            int id = await _categoriesConsumer.AddAsync(category);

            return id;
        }

        /// <summary>
        /// Asynchronously removed Category with specified id
        /// </summary>
        /// <param name="id">Id of the Category to delete</param>
        /// <returns>Task with success flag</returns>
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            bool success = await _categoriesConsumer.DeleteAsync(id);

            return success;
        }

        /// <summary>
        /// Asynchronously edits specified Category
        /// </summary>
        /// <param name="category">Category, that contains id of entity that should be changed, and all changed values</param>
        /// <returns>Task with success flag</returns>
        public async Task<bool> EditCategoryAsync(Category category)
        {
            bool success = await _categoriesConsumer.EditAsync(category);

            return success;
        }

        /// <summary>
        /// Asynchronously gets all Categories
        /// </summary>
        /// <returns>Task with list of all Categories</returns>
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            IEnumerable<Category> categories = await _categoriesConsumer.GetAllAsync();

            return categories;
        }

        /// <summary>
        /// Asynchronously gets Category with the specific id
        /// </summary>
        /// <param name="id">Id of the Category to get</param>
        /// <returns>Task with specified Category</returns>
        public async Task<Category> GetCategoryAsync(int id)
        {
            Category category = await _categoriesConsumer.GetAsync(id);

            return category;
        }

        /// <summary>
        /// Asynchronously get Category with the specific id and Products which are connected to the specified Category
        /// </summary>
        /// <param name="id">Id of the Category to get</param>
        /// <returns>Specified Category and list of related Products</returns>
        public async Task<CategoryProductsDTO> GetCategoryWithProductsAsync(int id)
        {
            Task<Category> getCategoryTask = _categoriesConsumer.GetAsync(id);
            Task<IEnumerable<Product>> getAllProductsTask = _productsConsumer.GetAllAsync();
            List<Task> taskList = new List<Task>
            {
                getCategoryTask,
                getAllProductsTask
            };

            await Task.WhenAll(taskList);

            Category category = getCategoryTask.Result;
            List<Product> relatedProducts = getAllProductsTask.Result?.Where(product => product.CategoryId == id).ToList();

            CategoryProductsDTO dto = new CategoryProductsDTO
            {
                Category = category,
                Products = relatedProducts
            };

            return dto;
        }
        #endregion

        #region Products
        /// <summary>
        /// Asynchronously add Product
        /// </summary>
        /// <param name="Product">Product to add</param>
        /// <returns>Task with Product id</returns>
        public async Task<int> AddProductAsync(Product product)
        {
            int id = await _productsConsumer.AddAsync(product);
            return id;
        }

        /// <summary>
        /// Asynchronously remove Product with specified id
        /// </summary>
        /// <param name="id">Id of the Product to delete</param>
        /// <returns>Task with success flag</returns>
        public async Task<bool> DeleteProductAsync(int id)
        {
            bool success = await _productsConsumer.DeleteAsync(id);

            return success;
        }

        /// <summary>
        /// Asynchronously edit specified Product
        /// </summary>
        /// <param name="Product">Product, that contains id of entity that should be changed, and all changed values</param>
        /// <returns>Task with success flag</returns>
        public async Task<bool> EditProductAsync(Product product)
        {
            bool success = await _productsConsumer.EditAsync(product);

            return success;
        }

        /// <summary>
        /// Asynchronously get all Products
        /// </summary>
        /// <returns>Task with list of all Products</returns>
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            IEnumerable<Product> products = await _productsConsumer.GetAllAsync();

            return products;
        }

        /// <summary>
        /// Asynchronously get Product with the specific id
        /// </summary>
        /// <param name="id">Id of the Product to get</param>
        /// <returns>Task with specified Product</returns>
        public async Task<Product> GetProductAsync(int id)
        {
            Product product = await _productsConsumer.GetAsync(id);

            return product;
        }

        /// <summary>
        /// Asynchronously get Product with the specific id with the related Category
        /// </summary>
        /// <param name="id">Id of the Product to get</param>
        /// <returns>Specified Product with the related Category</returns>
        public async Task<Product> GetProductWithCategoryAsync(int id)
        {
            Product product = await _productsConsumer.GetAsync(id);
            if (product == null) return product;
            product.Category = await _categoriesConsumer.GetAsync(product.CategoryId);

            return product;
        }
        #endregion

        #region Comments
        /// <summary>
        ///Asynchronously Add Comment 
        /// </summary>
        /// <param name="comment">Comment to Add</param>
        /// <returns>Task with Comment Id</returns>
        public async Task<int> AddCommentAsync(Comment comment)
        {
            int result = await _commentsConsumer.AddAsync(comment);

            return result;
        }

        /// <summary>
        /// Asynchronously Delete Comment 
        /// </summary>
        /// <param name="id">Id of Comment to Delete</param>
        /// <returns>Task with boolean result</returns>
        public async Task<bool> DeleteCommentAsync(int id)
        {
            bool result = await _commentsConsumer.DeleteAsync(id);

            return result;
        }

        /// <summary>
        /// Asynchronously Edit comment's message
        /// <para>This method calls Edit(int, string) using Id and Message from the Comment</para>
        /// </summary>
        /// <param name="comment">Comment to edit</param>
        /// <returns>True if succeeded</returns>
        public async Task<bool> EditCommentAsync(int id, string message)
        {
            bool result = await _commentsConsumer.EditAsync(id, message);

            return result;
        }

        /// <summary>
        /// Asynchronously Edit comment's message
        /// <para>This method calls EditComment(int, string) using Id and Message from the Comment</para>
        /// </summary>
        /// <param name="comment">Comment to edit</param>
        /// <returns>True if succeeded</returns>
        public Task<bool> EditCommentAsync(Comment comment) => EditCommentAsync(comment.Id, comment.Message);

        /// <summary>
        /// Asynchronously Get all comments for requested product 
        /// </summary>
        /// <param name="productId">Product Id for product</param>
        /// <returns>Task with list of comments</returns>
        public async Task<IEnumerable<Comment>> GetAllProductCommentsAsync(int productID)
        {
            IEnumerable<Comment> comments = await _commentsConsumer.GetAllProductRelatedCommentsAsync(productID);

            return comments;
        }

        /// <summary>
        /// Asynchronously get Comment by Id
        /// </summary>
        /// <param name="id">Id for requested Comment</param>
        /// <returns>Task with Comment</returns>
        public async Task<Comment> GetCommentAsync(int id)
        {
            Comment comment = await _commentsConsumer.GetAsync(id);

            return comment;
        }
        #endregion
    }
}