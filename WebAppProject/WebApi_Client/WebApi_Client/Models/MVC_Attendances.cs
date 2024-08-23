using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi_Client.Models
{
    public class MVC_Attendances
    {
          
        [Key]
        public int AttendanceID { get; set; }

        [Required]
        public int EmployeeID { get; set; }

        [Required]
        public int ProjectID { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime AttendanceDate { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(50)]
        public string ApprovalStatus { get; set; }

        [Required]
        public int ManagerID { get; set; }

        // Navigation properties
        [ForeignKey("EmployeeID")]
        public virtual MVC_Employee Employee { get; set; }

        [ForeignKey("ProjectID")]
        public virtual MVC_Project Project { get; set; }

        [ForeignKey("ManagerID")]
        public virtual MVC_Employee Manager { get; set; }
    }
}
