using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Entities
{
    public class Session : BaseEntity
    {

        public string Description { get; set; } = null!;
        
        
        public int Capacity { get; set; }
        
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }

        #region 1:M RS Between Session And Category
        // Fk 
        public int CategoryId { get; set; }

        public Category SessionCategory { get; set; }

        #endregion

        #region 1:M RS Between Session And Trainer
        // Fk 
        public int TrainerId { get; set; }

        // Nav Property
        public Trainer SessionTrainer { get; set; }

        #endregion


        #region M:M RS Between Member and Session

        public ICollection<MemberSession> SessionMembers { get; set; }

        #endregion
    }
}
