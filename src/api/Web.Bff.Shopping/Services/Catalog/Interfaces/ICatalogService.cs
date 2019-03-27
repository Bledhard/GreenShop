﻿using Common.Models.Categories;
using Common.Models.Comments;
using Common.Models.DTO;
using Common.Models.Products;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenShop.Web.Bff.Shopping.Services.Interfaces
{
    public interface ICatalogService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryAsync(int id);
        Task<CategoryProductsDTO> GetCategoryWithProductsAsync(int id);
        Task<int> AddCategoryAsync(Category category);
        Task<bool> DeleteCategoryAsync(int id);
        Task<bool> EditCategoryAsync(Category category);

        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductAsync(int id);
        Task<Product> GetProductWithCategoryAsync(int id);
        Task<int> AddProductAsync(Product product);
        Task<bool> DeleteProductAsync(int id);
        Task<bool> EditProductAsync(Product product);

        Task<IEnumerable<Comment>> GetAllProductCommentsAsync(int productID);
        Task<Comment> GetCommentAsync(int id);
        Task<int> AddCommentAsync(Comment comment);
        Task<bool> EditCommentAsync(int id, string message);
        Task<bool> EditCommentAsync(Comment comment);
        Task<bool> DeleteCommentAsync(int id);
    }
}
