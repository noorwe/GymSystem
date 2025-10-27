using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Entities
{
    public class Category : BaseEntity
    {
        public string CategoryName { get; set; } = null!;


        #region 1:M RS Between Session And Category

        public ICollection<Session> Sessions { get; set; }

        #endregion
    }
}
