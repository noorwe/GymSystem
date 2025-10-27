using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels.PlanViewModels;
using GymSystemDAL.Entities;
using GymSystemDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services.Classes
{
    internal class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlanService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var Plans = _unitOfWork.GetRepository<Plan>().GetAll();
            if (Plans is null || !Plans.Any()) return [];

            return Plans.Select(P => new
            PlanViewModel 
            {
                Id = P.Id,
                Name = P.Name,
                Description = P.Description,
                DurationDays = P.DurationDays,
                Price = P.Price,
                IsActive = P.IsActive,
            }
            );


        }

        public PlanViewModel? GetPlanById(int id)
        {
            var Plan = _unitOfWork.GetRepository<Plan>().GetById(id);
            if (Plan == null) return null;

            var planViewModel = new PlanViewModel
            {
                Id = Plan.Id,
                Name = Plan.Name,
                Description = Plan.Description,
                DurationDays = Plan.DurationDays,
                Price = Plan.Price,
                IsActive = Plan.IsActive,
            };
            return planViewModel;
        }

        public UpdatePlanViewModel? GetUpdatePlanById(int id)
        {
            var Plan = _unitOfWork.GetRepository<Plan>().GetById(id);
            if (Plan is null || Plan.IsActive == false || HasActiveMembership(id)) return null;

            return new UpdatePlanViewModel()
            {
                Description = Plan.Description,
                PlanName = Plan.Name,
                DurationDays = Plan.DurationDays,
                Price = Plan.Price,
            };

        }

        public bool ToggleStatus(int PlanId)
        {
            var Repo = _unitOfWork.GetRepository<Plan>();
            var Plan = Repo.GetById(PlanId);
            if(Plan is null || HasActiveMembership(PlanId)) return false;
            Plan.IsActive = Plan.IsActive == true ? false : true;
            
            Plan.UpdatedAt = DateTime.Now;

            try
            {
                Repo.Update(Plan);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool UpdatePlan(int PlanId, UpdatePlanViewModel updatedPlan)
        {
            var Plan = _unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (Plan is null || HasActiveMembership(PlanId)) return false;

            try
            {
                Plan.Description = updatedPlan.Description;
                Plan.Price = updatedPlan.Price;
                Plan.DurationDays = updatedPlan.DurationDays;
                Plan.UpdatedAt = DateTime.Now;
                _unitOfWork.GetRepository<Plan>().Update(Plan);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }

        }


        #region Helper Methods

        private bool HasActiveMembership(int id)
        {
            var ActiveMembership = _unitOfWork.GetRepository<Membership>().GetAll
                (
                X => X.PlanId == id && X.Status == "Active"
                );
            return ActiveMembership.Any();
        }

        #endregion
    }
}
