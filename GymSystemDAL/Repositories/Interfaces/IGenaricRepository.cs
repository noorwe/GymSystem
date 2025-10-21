using GymSystemDAL.Entities;
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

        int Add(TEntity entity);

        int Update(TEntity entity);

        int Delete(TEntity entity);

    }
}
