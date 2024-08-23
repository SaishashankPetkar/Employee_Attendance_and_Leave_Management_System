using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace WebApi_Client.Models
{
    public class MVC_Project
    {
        [Key]
        public int ProjectID { get; set; }

        [Required]
        [StringLength(100)]
        public string ProjectName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(10)]
        public string Prj_status { get; set; }

        // Navigation properties
        public virtual ICollection<MVC_Employee> Employees { get; set; }
        public virtual ICollection<MVC_Attendances> Attendances { get; set; }
    }
}

    