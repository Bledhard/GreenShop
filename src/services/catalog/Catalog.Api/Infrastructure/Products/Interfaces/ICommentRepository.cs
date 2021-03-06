﻿using GreenShop.Catalog.Api.Domain.Products;
using GreenShop.Catalog.Api.Service.Products;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenShop.Catalog.Api.Infrastructure.Products.Interfaces
{
    public interface ICommentRepository : IRepository<Comment, CommentDto>
    {
        /// <summary>
        /// Asynchronously add List of the Comments
        /// </summary>
        /// <param name="comments">List of the Comments to insert to the database</param>
        /// <returns>Task with summary result for all list</returns>
        Task<IEnumerable<int>> CreateAsync(IEnumerable<Comment> comments);

        /// <summary>
        /// Asynchronously Edit comment's message
        /// </summary>
        /// <param name="id">Id of the Comment to edit</param>
        /// <param name="message">Updated message for the Comment</param>
        /// <returns>Task with number of proceeded rows</returns>
        Task<bool> UpdateAsync(int id, string message);

        /// <summary>
        /// Asynchronously Get all Comments by product ID
        /// </summary>
        /// <param name="productId">Id of the product to get its comments</param>
        /// <returns>Task with list of comments</returns>
        Task<IEnumerable<Comment>> GetAllParentRelatedAsync(int productId);

        /// <summary>
        /// Asynchronously Get all Comments for the Products, which IDs are presented in the list
        /// </summary>
        /// <param name="productIds">List of Ids of the products</param>
        /// <returns>Task with list of comments</returns>
        Task<Dictionary<int, IEnumerable<Comment>>> GetAllParentRelatedAsync(IEnumerable<int> productIds);

        /// <summary>
        /// Asynchronously Delete all Comments for the specified Product
        /// </summary>
        /// <param name="productId">Product Id</param>
        /// <returns>Result flag</returns>
        Task<bool> DeleteAllParentRelatedAsync(int productId);
    }
}
