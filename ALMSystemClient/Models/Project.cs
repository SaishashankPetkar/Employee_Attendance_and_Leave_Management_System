using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi_Client.Models
{
    public class Project
    {
        [Key]
       
        public int ProjectID { get; set; } // Primary Key
        public string ProjectName { get; set; } // Name of the project
        public string Description { get; set; } // Description of the project
        public string Prj_status { get; set; } //project status
    }
}
