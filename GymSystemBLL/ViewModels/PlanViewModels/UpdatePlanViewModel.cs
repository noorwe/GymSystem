using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.ViewModels.PlanViewModels
{
    public class UpdatePlanViewModel
    {

        public string PlanName { get; set; } = null!;

        [Required(ErrorMessage = "Description Is Required !")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Description Must Be In Range 5 to 200 Chars !" )]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "DurationDays Is Required !")]
        [Range(1, 365 , ErrorMessage = "DurationDays Must Be In Range 1 To 365")]
        public int DurationDays { get; set; }

        [Required(ErrorMessage = "Price Is Required !")]
        [Range(1, 10000, ErrorMessage = "Price Must Be In Range 1 To 10000")]
        public decimal Price { get; set; }
    }
}
