using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi_Client.Models
{
    public class Leave
    {
        [Key]
        public int LeaveID { get; set; }

        [Required]
        [ForeignKey("Employee")]
        public int EmployeeID { get; set; }

        [Required]
        [ForeignKey("Manager")]
        public int ManagerID { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required]
        [StringLength(50)]
        public string LeaveType { get; set; }

        [StringLength(255)]
        public string Reason { get; set; }

        [Required]
        [StringLength(50)]
        public string ApprovalStatus { get; set; }

        [Required]
        [StringLength(50)]
        public string LeaveStatus { get; set; } // or 'Active'

        // Navigation properties
        public virtual MVC_Employee Employee { get; set; }
        public virtual MVC_Employee Manager { get; set; }
    }
}
