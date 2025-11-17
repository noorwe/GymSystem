using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class TrainerController : Controller
    {
        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        public IActionResult Index()
        {
            var trainers = _trainerService.GetAllTrainers();
            return View(trainers);
        }

        public ActionResult TrainerDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Can Not Be 0 Or Negative Number !";
                return RedirectToAction(nameof(Index));
            }


            var trainerDetails = _trainerService.GetTrainerDetails(id);

            if (trainerDetails == null)
            {
                TempData["ErrorMessage"] = "Member Not Found !";
                return RedirectToAction(nameof(Index));
            }
            return View(trainerDetails);
        }



        public ActionResult TrainerEdit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Can Not Be 0 Or Negative Number !";
                return RedirectToAction(nameof(Index));
            }


            var Trainer = _trainerService.GetTrainerToUpdate(id);

            if (Trainer == null)
            {
                TempData["ErrorMessage"] = "Member Not Found !";
                return RedirectToAction(nameof(Index));
            }
            return View(Trainer);
        }

        [HttpPost]
        public ActionResult TrainerEdit([FromRoute] int id, TrainerToUpdateViewModel trainerToUpdate)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Data Invalid", "Check Data And Missing Fields !");
                return View(trainerToUpdate);
            }

            var Result = _trainerService.UpdateTrainerDetails(id, trainerToUpdate);

            if (Result)
            {
                TempData["SuccessMessage"] = "Trainer Updated Successfully !";
            }
            else
            {
                TempData["ErrorMessage"] = "Trainer Failed To Update !";
            }

            return RedirectToAction(nameof(Index));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateTrainer(CreateTrainerViewModel CreatedTrainer)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Data Invalid", "Check Data And Missing Fields !");
                return RedirectToAction(nameof(Create), CreatedTrainer);
            }

            var Result = _trainerService.CreateTrainer(CreatedTrainer);

            if (Result)
            {
                TempData["SuccessMessage"] = "Member Created Successfully !";
            }
            else
            {
                TempData["ErrorMessage"] = "Member Failed To Create !";

            }

            return RedirectToAction(nameof(Index));

        }


        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Can Not Be 0 Or Negative Number !";
                return RedirectToAction(nameof(Index));
            }

            var Trainer = _trainerService.GetTrainerDetails(id);
            if (Trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found !";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TrainerId = id;
            ViewBag.TrainerName = Trainer.Name;
            return View();
        }


        public ActionResult DeleteConfirmed([FromForm] int id)
        {

            var Result = _trainerService.RemoveTrainer(id);

            if (Result)
            {
                TempData["SuccessMessage"] = "Trainer Deleted Successfully !";
            }
            else
            {
                TempData["ErrorMessage"] = "Trainer Failed To Delete !";

            }

            return RedirectToAction(nameof(Index));

        }
    }
}
