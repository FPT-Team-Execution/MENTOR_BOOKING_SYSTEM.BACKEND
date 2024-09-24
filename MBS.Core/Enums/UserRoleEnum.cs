using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Core.Enums
{
    public enum UserRoleEnum
    {
        [Display(Name = "Admin")] Admin,
        [Display(Name = "Student")] Student,
        [Display(Name = "Mentor")] Mentor
    }
}