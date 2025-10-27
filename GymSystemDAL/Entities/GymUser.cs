﻿using GymSystemDAL.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Entities
{
    public abstract class GymUser : BaseEntity
    {
        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public DateOnly DateOfBirth { get; set; }

        public Gender Gender { get; set; }

        public Address Address { get; set; } = null!;



    }
    [Owned]
    public class Address() 
    {
        public int BuildingNumber { get; set; }

        public String Street { get; set; } = null!;

        public String City { get; set; } = null!;
    }
}
