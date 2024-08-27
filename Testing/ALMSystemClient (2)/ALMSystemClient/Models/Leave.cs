using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALMSystem2.Models
{
    public class Leave
    { 
        [Key]
        public int LeaveID { get; set; }

        public int EmployeeID { get; set; }

        
        public DateTime StartDate { get; set; }


        public DateTime EndDate { get; set; }

        
        public string LeaveType { get; set; }

        public string Reason { get; set; }
        public string ApprovalStatus { get; set; } 
        public int ManagerID { get; set; }
        public string Leave_Status { get; set; }


      
    }
}