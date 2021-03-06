﻿using GreenShop.Catalog.Api.Config.Interfaces;
using GreenShop.Catalog.Api.Domain.Categories;
using GreenShop.Catalog.Api.Infrastructure.Products.Interfaces;
using GreenShop.Catalog.Api.Service.Categories;
using MongoDB.Driver;
using System;
using System.Data;

namespace GreenShop.Catalog.Api.Infrastructure
{
    public class DomainScope : IDomainScope
    {
        private ISqlContext SqlContext;
        private readonly IMongoContext MongoContext;
        private IDbTransaction SqlTransaction;
        private IClientSessionHandle MongoSession;
        public ISqlProductRepository SqlProductRepository { get; private set; }
        public IMongoProductRepository MongoProductRepository { get; private set; }
        public IRepository<Category, CategoryDto> CategoryRepository { get; private set; }
        public ICommentRepository Comments { get; private set; }
        private bool _disposed = false;

        public DomainScope(ISqlContext sqlContext,
            IMongoContext mongoContext,
            ISqlProductRepository sqlProductRepository,
            IMongoProductRepository mongoProductRepository,
            IRepository<Category, CategoryDto> categoryRepository,
            ICommentRepository comments)
        {
            SqlContext = sqlContext;
            MongoContext = mongoContext;
            SqlProductRepository = sqlProductRepository;
            MongoProductRepository = mongoProductRepository;
            CategoryRepository = categoryRepository;
            Comments = comments;
        }

        /// <summary>
        /// Start Transactions in SQL and Mongo databases
        /// </summary>
        public void Begin()
        {
            SqlContext.Connection.Open();
            SqlTransaction = SqlContext.Connection.BeginTransaction();

            MongoSession = MongoContext.Client.StartSession();
            MongoSession.StartTransaction();

            SqlProductRepository.SetSqlTransaction(SqlTransaction);
            CategoryRepository.SetSqlTransaction(SqlTransaction);
        }

        /// <summary>
        /// Persist changes made inside the current transaction to the SQL and Mongo databases
        /// </summary>
        public void Commit()
        {
            SqlTransaction.Commit();
            MongoSession.CommitTransactionAsync();
        }

        /// <summary>
        /// Revert changes that were made throughout the current transaction
        /// </summary>
        public void Rollback()
        {
            SqlTransaction.Rollback();
            MongoSession.AbortTransactionAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (SqlTransaction != null)
                    {
                        SqlTransaction.Dispose();
                        SqlTransaction = null;
                    }
                    if (SqlContext != null)
                    {
                        SqlContext.Dispose();
                        SqlContext = null;
                    }
                    if (MongoSession != null)
                    {
                        MongoSession.Dispose();
                        MongoSession = null;
                    }
                }
                _disposed = true;
            }
        }

        ~DomainScope()
        {
            Dispose(false);
        }
    }
}
