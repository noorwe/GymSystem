using GymSystemDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Data.Configurations
{
    internal class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable(Tb =>
            {
                Tb.HasCheckConstraint("SessionCapacityCheck", "Capacity Between 1 and 25");

                Tb.HasCheckConstraint("SessionEndDateCheck", "EndDate > StartDate");
            });

            #region 1:M RS Between Session And Category
            builder.HasOne(X => X.SessionCategory)
                .WithMany(X => X.Sessions)
                .HasForeignKey(X => X.CategoryId);

            #endregion

            #region 1:M RS Between Session And Trainer

            builder.HasOne(X => X.SessionTrainer)
                .WithMany(X => X.TrainerSessions)
                .HasForeignKey(X => X.TrainerId);

            #endregion
        }
    }
}
