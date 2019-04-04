﻿using GreenShop.Catalog.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace GreenShop.Catalog.Infrastructure
{
    public interface IRepository<TAggregate> where TAggregate : IAggregate
    {
        IDbTransaction Transaction { get; }

        void SetSqlTransaction(IDbTransaction transaction);

        Task<IEnumerable<TAggregate>> GetAllAsync();
        Task<TAggregate> GetAsync(string id);
        Task<bool> DeleteAsync(string id);
        Task<bool> CreateAsync(TAggregate entity);
        Task<bool> UpdateAsync(TAggregate entity);
    }
}