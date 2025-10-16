using GymSystemDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Entities
{
    internal class Trainer : GymUser
    {
        // CreatedAt Column Existed in BaseEntity
        // I Will Use This Column as HireDate For Member => Configurations

        public Specialites Specialites { get; set; }

        #region 1:M RS Between Session And Trainer
       
        public ICollection<Session> TrainerSessions { get; set; }

        #endregion

    }
}
