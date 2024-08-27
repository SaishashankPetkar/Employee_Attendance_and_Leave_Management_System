using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALMSystem2.Models
{
    public class Attendance
    {
        public int AttendanceID { get; set; }
        public int EmployeeID { get; set; }
        public int ProjectID { get; set; }
        public DateTime AttendanceDate { get; set; }
        public string Status { get; set; }
        public string ApprovalStatus { get; set; }
        public int ManagerID { get; set; }
        public string Atd_Status { get; set; }
    }
}