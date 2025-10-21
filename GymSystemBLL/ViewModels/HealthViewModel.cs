using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GymSystemBLL.ViewModels
{
    public class HealthViewModel
    {
        [Required(ErrorMessage = "Height Is Required !")]
        [Range(30, 300, ErrorMessage = "Height Must Be Between 30 Cm And 300 Cm !")]
        public decimal Height { get; set; }

        [Required(ErrorMessage = "Weight Is Required !")]
        [Range(1, 500, ErrorMessage = "Weight Must Be Between 1 Kg And 500 Kg !")]
        public decimal Weight { get; set; }

        [Required(ErrorMessage = "Blood Type Is Required !")]
        [MaxLength(3, ErrorMessage = "Blood Type Must Be At Most 3 Characters !")]
        public string BloodType { get; set; } = null!;

        public string? Note { get; set; }
    }
}
