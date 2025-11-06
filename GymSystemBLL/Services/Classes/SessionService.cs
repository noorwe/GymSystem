using AutoMapper;
using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels.SessionViewModels;
using GymSystemDAL.Entities;
using GymSystemDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public bool CreateSession(CreateSessionViewModel createdSession)
        {
            try 
            {
                if (!IsTrainerExist(createdSession.TrainerId)) return false;
                if (!IsCategoryExist(createdSession.CategoryId)) return false;
                if (!IsDateTimeValid(createdSession.StartDate, createdSession.EndDate)) return false;
                if (createdSession.Capacity > 25 || createdSession.Capacity < 0 ) return false;

                var SessionEntity = _mapper.Map<Session>(createdSession);

                _unitOfWork.GetRepository<Session>().Add(SessionEntity);
                return _unitOfWork.SaveChanges() > 0;

            } catch (Exception) 
            { 
                return false;
            }
        }

        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var Sessions = _unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategory();

            if (!Sessions.Any()) return [];

            return Sessions.Select(S => new SessionViewModel
            {
                Id = S.Id,
                Description = S.Description,
                StartDate = S.StartDate,
                EndDate = S.EndDate,
                Capacity = S.Capacity,
                TrainerName = S.SessionTrainer.Name,
                CategoryName = S.SessionCategory.CategoryName,
                AvailableSlots = S.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedStats(S.Id),
            });
        }

        public SessionViewModel? GetSessionById(int sessionId)
        {
            var Session = _unitOfWork.SessionRepository.GetSessionWithTrainerAndCategory(sessionId);

            if (Session == null) return null;

            //return new SessionViewModel
            //{
            //    Id = Session.Id,
            //    Description = Session.Description,
            //    StartDate = Session.StartDate,
            //    EndDate = Session.EndDate,
            //    Capacity = Session.Capacity,
            //    TrainerName = Session.SessionTrainer.Name,
            //    CategoryName = Session.SessionCategory.CategoryName,
            //    AvailableSlots = Session.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedStats(S.Id),
            //}

            var MappedSession = _mapper.Map<Session, SessionViewModel>(Session);
            MappedSession.AvailableSlots = MappedSession.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedStats(MappedSession.Id);

            return MappedSession;
        }


        public UpdateSessionViewModel? GetSessionToUpdate(int sessionId)
        {
            var Session = _unitOfWork.SessionRepository.GetById(sessionId);
            if (!IsSessionAvilableForUpdate(Session!)) return null;

            return _mapper.Map<UpdateSessionViewModel>(Session);
        }

        public bool UpdateSession(UpdateSessionViewModel updatedSession, int sessionId)
        {
            try 
            {
                var Session = _unitOfWork.SessionRepository.GetById(sessionId);
                if (!IsSessionAvilableForUpdate(Session!)) return false;
                if (!IsTrainerExist(updatedSession.TrainerId)) return false;
                if (!IsDateTimeValid(updatedSession.StartDate, updatedSession.EndDate)) return false;

                _mapper.Map(updatedSession, Session);
                Session!.UpdatedAt = DateTime.Now; 
                _unitOfWork.SessionRepository.Update(Session);

                return _unitOfWork.SaveChanges() > 0;

            } catch(Exception) 
            {
                return false;
            }

        }



        public bool RemoveSession(int sessionId)
        {
            try 
            {
                var Session = _unitOfWork.SessionRepository.GetById(sessionId);
                if (!IsSessionAvilableForDelete(Session!)) return false;

                _unitOfWork.SessionRepository.Delete(Session!);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch(Exception) 
            {
                return false;
            }
        }



        #region Helper Methods

        private bool IsTrainerExist(int TrainerId) 
        {
            return _unitOfWork.GetRepository<Trainer>().GetById(TrainerId) is not null;
        }

        private bool IsCategoryExist(int CategoryId)
        {
            return _unitOfWork.GetRepository<Category>().GetById(CategoryId) is not null;
        }

        private bool IsDateTimeValid(DateTime StartDate, DateTime EndDate) 
        {
            return StartDate < EndDate;
        }

        private bool IsSessionAvilableForUpdate(Session session) 
        {
            if (session is null) return false;

            if (session.EndDate < DateTime.Now) return false;

            if (session.StartDate <= DateTime.Now) return false;

            var ActiveBooking = _unitOfWork.SessionRepository.GetCountOfBookedStats(session.Id ) > 0;
            if(ActiveBooking) return false;

            return true;
        }


        private bool IsSessionAvilableForDelete(Session session)
        {
            if (session is null) return false;

            if (session.EndDate < DateTime.Now) return false;

            if (session.StartDate > DateTime.Now) return false;

            if (session.StartDate <= DateTime.Now && session.EndDate > DateTime.Now) return false;

                var ActiveBooking = _unitOfWork.SessionRepository.GetCountOfBookedStats(session.Id) > 0;
            if (ActiveBooking) return false;

            return true;
        }


        #endregion
    }
}
