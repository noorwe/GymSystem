using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Entities
{
    public class Member : GymUser
    {
        // CreatedAt Column Existed in BaseEntity

        // I Will Use This Column as HireDate For Member => Configurations

        public string Photo { get; set; } = null!;


        #region 1:1 RS Between Member HealthRecord

        // Nav Property
        public HealthRecord HealthRecord { get; set; }

        #endregion


        #region M:M RS Between Member and Plan

        public ICollection<Membership> Memberships { get; set; }

        #endregion

        #region M:M RS Between Member and Session

        public ICollection<MemberSession> MemberSessions { get; set; }

        #endregion
    }
}
