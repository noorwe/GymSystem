﻿using GymSystemDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Data.Configurations
{
    internal class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(X => X.Name)
                .HasColumnName("varchar")
                .HasMaxLength(50);

            builder.Property(X => X.Email)
                .HasColumnName("varchar")
                .HasMaxLength(100);

            builder.Property(X => X.Phone)
                .HasColumnName("varchar")
                .HasMaxLength(11);

            builder.ToTable(Tb =>
            {
                Tb.HasCheckConstraint("GymUservalidEmailCheck" , "Email Like '_%@_%._&'");

                Tb.HasCheckConstraint("GymUservalidPhoneCheck", "Phone Like '01%' and Phone Not Like '%[^0-9]%'");

            });

            builder.HasIndex(X => X.Email).IsUnique();

            builder.HasIndex(X => X.Phone).IsUnique();

            builder.OwnsOne(X => X.Address, AddressBuilder =>
            {
                AddressBuilder.Property(X => X.Street)
                .HasColumnName("Street")
                .HasColumnType("varchar")
                .HasMaxLength(30);

                AddressBuilder.Property(X => X.City)
               .HasColumnName("City")
               .HasColumnType("varchar")
               .HasMaxLength(30);
            });


        }
    }
}
