using GymSystemBLL.ViewModels.PlanViewModels;
using GymSystemDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services.Interfaces
{
    internal interface IPlanService
    {
        IEnumerable<PlanViewModel> GetAllPlans();

        PlanViewModel? GetPlanById(int id);

        UpdatePlanViewModel? GetUpdatePlanById(int id);

        bool UpdatePlan(int PlanId, UpdatePlanViewModel updatedPlan);

        bool ToggleStatus(int PlanId);
    }
}
