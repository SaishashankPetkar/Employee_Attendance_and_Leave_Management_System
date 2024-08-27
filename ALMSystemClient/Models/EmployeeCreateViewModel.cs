using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALMSystem2.Models
{
    public class EmployeeCreateViewModel
    {
        public MVCEmployees Employee { get; set; }
        public IEnumerable<Role> Roles { get; set; }
        public IEnumerable<Project> Projects { get; set; }
    }
}