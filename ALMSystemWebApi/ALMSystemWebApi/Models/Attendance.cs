//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ALMSystemWebApi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Attendance
    {
        public int AttendanceID { get; set; }
        public int EmployeeID { get; set; }
        public int ProjectID { get; set; }
        public System.DateTime AttendanceDate { get; set; }
        public string Status { get; set; }
        public string ApprovalStatus { get; set; }
        public int ManagerID { get; set; }
        public string Atd_Status { get; set; }
    
        public virtual Employee Employee { get; set; }
        public virtual Employee Employee1 { get; set; }
        public virtual Project Project { get; set; }
    }
}
