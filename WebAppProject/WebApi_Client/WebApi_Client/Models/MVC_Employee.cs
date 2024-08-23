using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace WebApi_Client.Models
{
    public class MVC_Employee
    {
        [Key]
        public int EmployeeID { get; set; }

        [Required]
        [StringLength(50)]
        public string EmployeeName { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(13)]
        public string Phone { get; set; }

        [Required]
        public DateTime HireDate { get; set; }

        [Required]
        public int RoleID { get; set; }

        public int? ManagerID { get; set; }

        [Required]
        public int ProjectID { get; set; }

        public int LeaveBalance { get; set; }

        public int No_of_leave { get; set; }

        [StringLength(10)]
        public string Emp_status { get; set; }

        [Required]
        [StringLength(256)]
        public string Password { get; set; }

        // Navigation properties
        public virtual Role Role { get; set; }
        public virtual MVC_Project Project { get; set; }
        public virtual MVC_Employee Manager { get; set; }
        public virtual ICollection<MVC_Attendances> Attendances { get; set; }
        public virtual ICollection<Leave> Leaves { get; set; }
    }
}
    