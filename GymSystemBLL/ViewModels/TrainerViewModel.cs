﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.ViewModels
{
    public class TrainerViewModel
    {
        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string Specialization { get; set; } = null!;

        // Details [Address, Birthdate ]

        public string Address { get; set; } = null!;

        public string DateOfBirth { get; set; } = null!;
    }
}
