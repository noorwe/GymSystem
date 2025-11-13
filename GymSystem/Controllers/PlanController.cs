using GymSystemBLL.Services.Classes;
using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels;
using GymSystemBLL.ViewModels.PlanViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanService _planService;

        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }
        public IActionResult Index()
        {
            var Plans = _planService.GetAllPlans();
            return View(Plans);
        }

        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Can Not Be 0 Or Negative Number !";
                return RedirectToAction(nameof(Index));
            }


            var planDetails = _planService.GetPlanById(id);

            if (planDetails == null)
            {
                TempData["ErrorMessage"] = "Plan Not Found !";
                return RedirectToAction(nameof(Index));
            }
            return View(planDetails);
        }



        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Can Not Be 0 Or Negative Number !";
                return RedirectToAction(nameof(Index));
            }


            var Plan = _planService.GetUpdatePlanById(id);

            if (Plan == null)
            {
                TempData["ErrorMessage"] = "Plan Not Found !";
                return RedirectToAction(nameof(Index));
            }
            return View(Plan);
        }

        [HttpPost]
        public ActionResult Edit([FromRoute] int id, UpdatePlanViewModel planToUpdate)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Data Invalid", "Check Data And Missing Fields !");
                return View(planToUpdate);
            }

            var Result = _planService.UpdatePlan(id, planToUpdate);

            if (Result)
            {
                TempData["SuccessMessage"] = "Plan Updated Successfully !";
            }
            else
            {
                TempData["ErrorMessage"] = "Plan Failed To Update !";
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public ActionResult Activate(int id) 
        {
            var Result = _planService.ToggleStatus(id);

            if (Result)
            {
                TempData["SuccessMessage"] = "Plan Status Changed Successfully !";
                return RedirectToAction(nameof(Index));

            }
            else
            {
                TempData["ErrorMessage"] = "Plan Failed To Change Status !";
                return RedirectToAction(nameof(Index));

            }

        }

    }
}
