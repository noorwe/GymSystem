using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels;
using GymSystemDAL.Entities;
using GymSystemDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services.Classes
{
    internal class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MemberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // public IGenaricRepository<Membership> MembershipRepository { get; }

        public bool CreateMember(CreateMemberViewModel createdMember)
        {
            try
            {
                if (IsEmailExist(createdMember.Email) || IsPhoneExist(createdMember.Phone)) return false;
              
                var member = new Member()
                {
                    Name = createdMember.Name,
                    Email = createdMember.Email,
                    Phone = createdMember.Phone,
                    DateOfBirth = createdMember.DateOfBirth,
                    Gender = createdMember.Gender,
                    Address = new Address()
                    {
                        BuildingNumber = createdMember.BuildingNumber,
                        Street = createdMember.Street,
                        City = createdMember.City
                    },
                    HealthRecord = new HealthRecord()
                    {
                        Weight = createdMember.HealthViewModel.Weight,
                        Height = createdMember.HealthViewModel.Height,
                        BloodType = createdMember.HealthViewModel.BloodType,
                        Note = createdMember.HealthViewModel.Note
                    }
                };
                _unitOfWork.GetRepository<Member>().Add(member);

                return _unitOfWork.SaveChanges() > 0;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            #region First Way Using LINQ Query Syntax
            //    var members = _memberRepository.GetAll() ?? [];

            //    var memberViewModels = new List<MemberViewModel>();
            //    foreach (var m in members)
            //    {
            //        var memberViewModel = new MemberViewModel
            //        {
            //            Id = m.Id,
            //            Name = m.Name,
            //            Photo = m.Photo,
            //            Email = m.Email,
            //            Phone = m.Phone,
            //            Gender = m.Gender.ToString()
            //        };
            //        memberViewModels.Add(memberViewModel);

            //    }
            //    return memberViewModels;

            #endregion


            #region Second Way Using LINQ Method Syntax

            var Members = _unitOfWork.GetRepository<Member>().GetAll();
            if (Members == null || Members.Any()) return [];
                
            var memberViewModels = Members.Select(m => new MemberViewModel
            {
                Id = m.Id,
                Name = m.Name,
                Photo = m.Photo,
                Email = m.Email,
                Phone = m.Phone,
                Gender = m.Gender.ToString()

            });
            return memberViewModels;

            #endregion
        }

        public MemberViewModel? GetMemberDetails(int MemberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (member == null) return null;
            var memberViewModel = new MemberViewModel()
            {
                Id = member.Id,
                Name = member.Name,
                Photo = member.Photo,
                Email = member.Email,
                Phone = member.Phone,
                Gender = member.Gender.ToString(),
                DateOfBirth = member.DateOfBirth.ToShortDateString(),
                Address = $"{member.Address.BuildingNumber} - {member.Address.Street} - {member.Address.City}"
            };
            var ActiveMembership = _unitOfWork.GetRepository<Membership>().GetAll(m => m.MemberId == MemberId && m.Status == "Active").FirstOrDefault();

            if (ActiveMembership != null)
            {
                var plan = _unitOfWork.GetRepository<Plan>().GetById(ActiveMembership.PlanId);
                memberViewModel.PlanName = plan?.Name;
                memberViewModel.MembershipStartDate = ActiveMembership.CreatedAt.ToShortDateString();
                memberViewModel.MembershipEndDate = ActiveMembership.EndDate.ToShortDateString();
            }

            return memberViewModel;

        }

        public HealthViewModel? GetMemberHealthRecordDetails(int MemberId)
        {
            var MemberHealthRecord = _unitOfWork.GetRepository<HealthRecord>().GetById(MemberId);
            if (MemberHealthRecord == null) return null;
            var healthViewModel = new HealthViewModel()
            {
                Weight = MemberHealthRecord.Weight,
                Height = MemberHealthRecord.Height,
                BloodType = MemberHealthRecord.BloodType,
                Note = MemberHealthRecord.Note
            };
            return healthViewModel;
        }

        public MemberToUpdateViewModel? GetMemberToUpdate(int MemberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (member == null) return null;
            var memberToUpdate = new MemberToUpdateViewModel()
            {
                Name = member.Name,
                Photo = member.Photo,
                Email = member.Email,
                Phone = member.Phone,
                BuildingNumber = member.Address.BuildingNumber,
                Street = member.Address.Street,
                City = member.Address.City
            };
            return memberToUpdate;
        }

        public bool RemoveMember(int MemberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (member == null)
            {
                return false;
            }
            else
            {
                var hasActiveMemberSession = _unitOfWork.GetRepository<MemberSession>().GetAll(ms => ms.MemberId == MemberId && ms.Session.StartDate > DateTime.Now).Any();
                if (hasActiveMemberSession) return false;

                var memberships = _unitOfWork.GetRepository<Membership>().GetAll(m => m.MemberId == MemberId);

                try
                {
                    if(memberships.Any())
                    {
                        foreach (var membership in memberships)
                        {
                            _unitOfWork.GetRepository<Membership>().Delete(membership);
                        }
                    }
                    _unitOfWork.GetRepository<Member>().Delete(member);
                    return _unitOfWork.SaveChanges() > 0;
                }
                catch (Exception)
                {
                    return false;
                }



            }
        }

        public bool UpdateMemberDetails(int MemberId, MemberToUpdateViewModel updateMember)
        {
            try
            {

                if (IsEmailExist(updateMember.Email) || IsPhoneExist(updateMember.Phone)) return false;

                var Member = _unitOfWork.GetRepository<Member>().GetById(MemberId);
                if (Member == null) return false;

                Member.Email = updateMember.Email;
                Member.Phone = updateMember.Phone;
                Member.Address.BuildingNumber = updateMember.BuildingNumber;
                Member.Address.Street = updateMember.Street;
                Member.Address.City = updateMember.City;
                Member.UpdatedAt = DateTime.Now;
                _unitOfWork.GetRepository<Member>().Update(Member);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }


        private bool IsEmailExist(string email) 
        {
            return _unitOfWork.GetRepository<Member>().GetAll(m => m.Email == email).Any();

        }

        private bool IsPhoneExist(string phone)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(m => m.Phone == phone).Any();

        }
    }
}
