using GymSystemDAL.Data.Context;
using GymSystemDAL.Entities;
using GymSystemDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace GymSystemDAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymSystemDbContext _dbContext;
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>(); 

        public UnitOfWork(GymSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IGenaricRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var EntityType = typeof(TEntity);

            if (_repositories.TryGetValue(EntityType, out var Repo))
                return (IGenaricRepository<TEntity>) Repo;

            var NewRepo = new GenaricRepository<TEntity>(_dbContext);

            _repositories[EntityType] = NewRepo;
            return NewRepo;
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }
    }
}
