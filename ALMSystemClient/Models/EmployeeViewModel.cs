using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALMSystem2.Models
{
    public class EmployeeViewModel
    {
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime HireDate { get; set; }
        public string RoleID { get; set; }
        public string ManagerID { get; set; }
        public string ProjectID { get; set; }
        public int LeaveBalance { get; set; }
        public int No_of_leave { get; set; }
        public string Emp_status { get; set; }
    }
}