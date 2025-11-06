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
    public class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(X => X.Name)
                .HasColumnName("Name")
                .HasColumnType("varchar(50)");

            builder.Property(X => X.Email)
                .HasColumnName("Email")
                .HasColumnType("varchar(100)");

            builder.Property(X => X.Phone)
                .HasColumnName("Phone")
                .HasColumnType("varchar(11)");

            builder.ToTable(Tb =>
            {
                Tb.HasCheckConstraint("GymUservalidEmailCheck", "Email LIKE '_%@_%._%'");
                Tb.HasCheckConstraint("GymUservalidPhoneCheck", "Phone LIKE '01%' AND Phone NOT LIKE '%[^0-9]%'");
            });


            builder.HasIndex(X => X.Email).IsUnique();

            builder.HasIndex(X => X.Phone).IsUnique();

            builder.OwnsOne(X => X.Address, AddressBuilder =>
            {
                AddressBuilder.Property(X => X.Street)
                .HasColumnName("Street")
                .HasColumnType("varchar(30)");


                AddressBuilder.Property(X => X.City)
               .HasColumnName("City")
               .HasColumnType("varchar(30)");
               
            });


        }
    }
}
