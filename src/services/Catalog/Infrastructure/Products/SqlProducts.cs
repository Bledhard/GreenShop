﻿using Dapper;
using Dapper.Contrib.Extensions;
using GreenShop.Catalog.Config.Interfaces;
using GreenShop.Catalog.Infrastructure.Products.Interfaces;
using GreenShop.Catalog.Models.Products;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace GreenShop.Catalog.Infrastructure.Products
{
    public class SqlProducts : ISqlProducts
    {
        private readonly ISqlContext _sql;

        public IDbTransaction Transaction { get; private set; }

        public SqlProducts(ISqlContext sqlContext)
        {
            _sql = sqlContext;
        }

        public void SetSqlTransaction(IDbTransaction transaction)
        {
            Transaction = transaction;
        }

        /// <summary>
        /// Asynchronously get all Products
        /// </summary>
        /// <returns>Task with list of all Products</returns>
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            using (SqlConnection context = _sql.Connection)
            {
                IEnumerable<Product> products = await context.GetAllAsync<Product>();

                return products;
            }
        }

        /// <summary>
        /// Asynchronously get Product with the specific id
        /// </summary>
        /// <param name="id">Id of the Product to get</param>
        /// <returns>Task with specified Product</returns>
        public async Task<Product> GetAsync(string id)
        {
            using (SqlConnection context = _sql.Connection)
            {
                Product product = await context.GetAsync<Product>(id);

                return product;
            }
        }

        /// <summary>
        /// Asynchronously create Product
        /// </summary>
        /// <param name="product">Product to add</param>
        /// <returns>Result flag</returns>
        public async Task<bool> CreateAsync(Product product)
        {
            using (SqlConnection context = _sql.Connection)
            {
                await context.InsertAsync(product);

                return true;
            }
        }

        /// <summary>
        /// Asynchronously remove Product with specified id
        /// </summary>
        /// <param name="id">Id of the Product to delete</param>
        /// <returns>Result flag</returns>
        public async Task<bool> DeleteAsync(string id)
        {
            using (SqlConnection context = _sql.Connection)
            {
                await context.ExecuteAsync(@"
                    DELETE
                    FROM [Products]
                    WHERE [Id] = @id
                ", new
                {
                    id
                });

                return true;
            }
        }

        /// <summary>
        /// Asynchronously edit specified Product
        /// </summary>
        /// <param name="product">Product, that contains id of entity that should be changed, and all changed values</param>
        /// <returns>Result flag</returns>
        public async Task<bool> UpdateAsync(Product product)
        {
            using (SqlConnection context = _sql.Connection)
            {
                string query = @"
                    UPDATE [Products]
                    SET
                    ";

                if (!string.IsNullOrWhiteSpace(product.Name))
                {
                    query += " [Name] = @name";
                }
                if (product.CategoryId != null)
                {
                    query += " [CategoryId] = @categoryId";
                }
                if (!string.IsNullOrWhiteSpace(product.Description))
                {
                    query += " [Description] = @description";
                }
                if (product.BasePrice != 0)
                {
                    query += " [BasePrice] = @basePrice";
                }
                if (product.Rating != 0)
                {
                    query += " [Rating] = @rating";
                }

                query += " WHERE [Id] = @id";

                int affectedRows = await context.ExecuteAsync(query, new
                {
                    id = product.Id,
                    name = product.Name,
                    parentId = product.CategoryId,
                    description = product.Description,
                    basePrice = product.BasePrice,
                    rating = product.Rating
                });

                return true;
            }
        }
    }
}