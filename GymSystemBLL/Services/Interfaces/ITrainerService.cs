using GymSystemBLL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services.Interfaces
{
    public interface ITrainerService
    {

        IEnumerable<TrainerViewModel> GetAllTrainers();

        bool CreateTrainer(CreateTrainerViewModel createdTrainer);

        TrainerViewModel? GetTrainerDetails(int TrainerId);

        TrainerToUpdateViewModel? GetTrainerToUpdate(int TrainerId);

        bool UpdateTrainerDetails(int TrainerId, TrainerToUpdateViewModel updateTrainer);

        bool RemoveTrainer(int TrainerId);

    }
}
