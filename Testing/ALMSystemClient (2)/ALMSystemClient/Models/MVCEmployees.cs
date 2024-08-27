using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALMSystem2.Models
{
    public class Role
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
    }
    public class Project
    {
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public string Prj_status { get; set; }
    }
      
    public class MVCEmployees
    {
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime HireDate { get; set; }
        public int RoleID { get; set; }
        public Role Role { get; set; }
        public int? ManagerID { get; set; }
        public int ProjectID { get; set; }
        public Project Project { get; set; }
        public int? LeaveBalance { get; set; }
        public int? No_of_leave { get; set; }
        public string Emp_status { get; set; }
        public string Password { get; set; }
    }
}