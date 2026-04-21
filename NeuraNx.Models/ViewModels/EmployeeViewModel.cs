using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuraNx.Models.ViewModels
{
    public class EmployeeViewModel : BaseViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        public required string Name { get; set; }
        public required string Designation { get; set; }
        [EmailAddress(ErrorMessage = "Email is valid Email Format")]
        public required string Email { get; set; }
        [Phone(ErrorMessage = "Phone is valid Phone Format")]
        public string? Phone { get; set; }
    }
}
