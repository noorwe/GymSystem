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
    internal class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TrainerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool CreateTrainer(CreateTrainerViewModel createdTrainer)
        {
            try
            {
                if (IsEmailExist(createdTrainer.Email) || IsPhoneExist(createdTrainer.Phone)) return false;
                var trainer = new Trainer()
                {
                    Name = createdTrainer.Name,
                    Email = createdTrainer.Email,
                    Phone = createdTrainer.Phone,
                    DateOfBirth = createdTrainer.DateOfBirth,
                    Gender = createdTrainer.Gender,
                    Address = new Address()
                    {
                        BuildingNumber = createdTrainer.BuildingNumber,
                        Street = createdTrainer.Street,
                        City = createdTrainer.City
                    },
                    Specialites = createdTrainer.Specialization,
                };
                _unitOfWork.GetRepository<Trainer>().Add(trainer);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
            if(trainers == null || trainers.Any()) return [];

            var trainerViewModels = trainers.Select(t => new TrainerViewModel
            {
                Name = t.Name,
                Email = t.Email,
                Phone = t.Phone,
                Specialization = t.Specialites.ToString()
            });
            return trainerViewModels;
        }

        public TrainerViewModel? GetTrainerDetails(int TrainerId)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(TrainerId);
            if (trainer == null) return null;
            var trainerViewModel = new TrainerViewModel
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                Specialization = trainer.Specialites.ToString(),
                DateOfBirth = trainer.DateOfBirth.ToShortDateString(),
                Address = $"{trainer.Address.BuildingNumber} - {trainer.Address.Street} - {trainer.Address.City}",
            };
            return trainerViewModel;
        }

        public TrainerToUpdateViewModel? GetTrainerToUpdate(int TrainerId)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(TrainerId);
            if (trainer == null) return null;
            var trainerToUpdate = new TrainerToUpdateViewModel
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                BuildingNumber = trainer.Address.BuildingNumber,
                Street = trainer.Address.Street,
                City = trainer.Address.City,
                Specialization = trainer.Specialites,
            };
            return trainerToUpdate;
        }

        public bool RemoveTrainer(int trainerId)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);
            if (trainer == null)
            {
                return false; 
            }

            // Check if the trainer has any future sessions
            var hasFutureSession = _unitOfWork.GetRepository<Session>()
                .GetAll(s => s.TrainerId == trainerId && s.StartDate > DateTime.Now)
                .Any();

            if (hasFutureSession)
            {
                return false; 
            }
            try
            {

               _unitOfWork.GetRepository<Trainer>().Delete(trainer);
                return _unitOfWork.SaveChanges() > 0;

            }
            catch (Exception)
            {
                return false;
            }
            

        }


        public bool UpdateTrainerDetails(int TrainerId, TrainerToUpdateViewModel updateTrainer)
        {
            if (IsEmailExist(updateTrainer.Email) || IsPhoneExist(updateTrainer.Phone)) return false;

            try
            {
                var trainer = _unitOfWork.GetRepository<Trainer>().GetById(TrainerId);
                if (trainer == null) return false;
                trainer.Name = updateTrainer.Name;
                trainer.Email = updateTrainer.Email;
                trainer.Phone = updateTrainer.Phone;
                trainer.Address.BuildingNumber = updateTrainer.BuildingNumber;
                trainer.Address.Street = updateTrainer.Street;
                trainer.Address.City = updateTrainer.City;
                trainer.Specialites = updateTrainer.Specialization;
                trainer.UpdatedAt = DateTime.Now;
                _unitOfWork.GetRepository<Trainer>().Update(trainer);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }







        #region Private Methods

        private bool IsEmailExist(string email)
        {
            return _unitOfWork.GetRepository<Trainer>().GetAll(m => m.Email == email).Any();

        }

        private bool IsPhoneExist(string phone)
        {
            return _unitOfWork.GetRepository<Trainer>().GetAll(m => m.Phone == phone).Any();

        }
        #endregion
    }
}
