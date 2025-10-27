﻿using GymSystemDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Repositories.Interfaces
{
    public interface IGenaricRepository<TEntity> where TEntity : BaseEntity , new()
    {
        IEnumerable<TEntity> GetAll(Func<TEntity, bool>? condition = null);

        TEntity? GetById(int id);

        void Add(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

    }
}
