using GymSystemBLL.Services.Classes;
using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels;
using GymSystemBLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace GymSystem.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }
        public IActionResult Index()
        {
            var Sessions = _sessionService.GetAllSessions();
            return View(Sessions);
        }

        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Can Not Be 0 Or Negative Number !";
                return RedirectToAction(nameof(Index));
            }


            var session = _sessionService.GetSessionById(id);

            if (session == null)
            {
                TempData["ErrorMessage"] = "Session Not Found !";
                return RedirectToAction(nameof(Index));
            }
            return View(session);
        }


        public ActionResult Create()
        {
            LoadDropDownsTrainer();
            LoadDropDownsCategory();
            return View();
        }

        public ActionResult CreateSession(CreateSessionViewModel CreatedSession) 
        {
            if (!ModelState.IsValid)
            {

                LoadDropDownsTrainer();
                LoadDropDownsCategory();
                return View(CreatedSession);
            }

            var Result = _sessionService.CreateSession(CreatedSession);

            if (Result)
            {
                TempData["SuccessMessage"] = "Session Created Successfully !";
                LoadDropDownsTrainer();
                LoadDropDownsCategory();
                return RedirectToAction(nameof(Index));

            }
            else
            {
                TempData["ErrorMessage"] = "Session Failed To Create !";
                LoadDropDownsTrainer();
                LoadDropDownsCategory();
                return View(CreatedSession);
            }

        }


        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Can Not Be 0 Or Negative Number !";
                return RedirectToAction("Index");
            }


            var Session = _sessionService.GetSessionToUpdate(id);

            if (Session == null)
            {
                TempData["ErrorMessage"] = "Session Not Found !";
                return RedirectToAction("Index");
            }
            LoadDropDownsTrainer();
            return View(Session);
        }


        [HttpPost]
        public ActionResult Edit([FromRoute] int id, UpdateSessionViewModel UpdatedSession)
        {
            if (!ModelState.IsValid)
            {
                LoadDropDownsTrainer();
                return View(UpdatedSession);
            }

            var Result = _sessionService.UpdateSession(UpdatedSession, id);

            if (Result)
            {
                TempData["SuccessMessage"] = "Session Updated Successfully !";
                return RedirectToAction("Index");

            }
            else
            {
                TempData["ErrorMessage"] = "Session Failed To Update !";
                LoadDropDownsTrainer();
                return View(UpdatedSession);

            }

        }


        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Can Not Be 0 Or Negative Number !";
                return RedirectToAction(nameof(Index));
            }

            var Session = _sessionService.GetSessionById(id);
            if (Session == null)
            {
                TempData["ErrorMessage"] = "Member Not Found !";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.SessionId = id;
            return View();
        }


        public ActionResult DeleteConfirmed([FromForm] int id)
        {

            var Result = _sessionService.RemoveSession(id);

            if (Result)
            {
                TempData["SuccessMessage"] = "Session Deleted Successfully !";
            }
            else
            {
                TempData["ErrorMessage"] = "Session Failed To Delete !";

            }

            return RedirectToAction(nameof(Index));

        }



        private void LoadDropDownsTrainer() 
        {
            var Trainers = _sessionService.GetTrainerForSessons();
            ViewBag.Trainers = new SelectList(Trainers, "Id", "Name");
        }

        private void LoadDropDownsCategory()
        {
            var Categories = _sessionService.GetCategoryForSessons();
            ViewBag.Categories = new SelectList(Categories, "Id", "Name");
        }
    }
}
