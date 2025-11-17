using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GymSystem.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService) 
        {
            _memberService = memberService;
        }

        public IActionResult Index()
        {
            var members = _memberService.GetAllMembers();
            return View(members);
        }

        public ActionResult MemberDetails(int id) 
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Can Not Be 0 Or Negative Number !";
                return RedirectToAction(nameof(Index));
            }
               

            var memberDetails = _memberService.GetMemberDetails(id);

            if (memberDetails == null)
            {
                TempData["ErrorMessage"] = "Member Not Found !";
                return RedirectToAction(nameof(Index));
            }
            return View(memberDetails);
        }

        public ActionResult HealthRecordDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Can Not Be 0 Or Negative Number !";
                return RedirectToAction(nameof(Index));
            }
            var HealthRecord = _memberService.GetMemberHealthRecordDetails(id);

            if (HealthRecord == null)
            {
                TempData["ErrorMessage"] = "Member Not Found !";
                return RedirectToAction(nameof(Index));
            }
            return View(HealthRecord);
        }

        public ActionResult MemberEdit(int id) 
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Can Not Be 0 Or Negative Number !";
                return RedirectToAction(nameof(Index));
            }


            var Member = _memberService.GetMemberToUpdate(id);

            if (Member == null)
            {
                TempData["ErrorMessage"] = "Member Not Found !";
                return RedirectToAction(nameof(Index));
            }
            return View(Member);
        }

        [HttpPost]
        public ActionResult MemberEdit([FromRoute]int id, MemberToUpdateViewModel memberToUpdate) 
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("Data Invalid", "Check Data And Missing Fields !");
                return View(memberToUpdate);
            }

            var Result = _memberService.UpdateMemberDetails(id, memberToUpdate);

            if (Result) 
            {
                TempData["SuccessMessage"] = "Member Updated Successfully !";
            }
            else 
            {
                TempData["ErrorMessage"] = "Member Failed To Update !";
            }

            return RedirectToAction(nameof(Index));
        }

        public ActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateMember(CreateMemberViewModel CreatedMember) 
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Data Invalid", "Check Data And Missing Fields !");
                return RedirectToAction(nameof(Create), CreatedMember);
            }

            var Result = _memberService.CreateMember(CreatedMember);

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

            var Member = _memberService.GetMemberDetails(id);
            if (Member == null)
            {
                TempData["ErrorMessage"] = "Member Not Found !";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MemberId = id;
            ViewBag.MemberName = Member.Name;
            return View();
        }


        public ActionResult DeleteConfirmed([FromForm]int id) 
        {

            var Result = _memberService.RemoveMember(id);

            if (Result)
            {
                TempData["SuccessMessage"] = "Member Deleted Successfully !";
            }
            else
            {
                TempData["ErrorMessage"] = "Member Failed To Delete !";

            }

            return RedirectToAction(nameof(Index));

        }
    }
}
