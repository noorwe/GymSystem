using GymSystemDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Repositories.Interfaces
{
    public interface ISessionRepository : IGenaricRepository<Session>
    {
        IEnumerable<Session> GetAllSessionsWithTrainerAndCategory();

        int GetCountOfBookedStats(int sessionId);

        Session? GetSessionWithTrainerAndCategory(int sessionId);
    }
}
