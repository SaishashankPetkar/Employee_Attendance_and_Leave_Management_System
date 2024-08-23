using System.ComponentModel.DataAnnotations;

namespace WebApi_Client.Models
{
    public class Role
    {
        [Key]
        public int RoleID { get; set; }

        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }

        // Optional: Navigation property if you need to reference other entities related to this Role
        // public virtual ICollection<Employee> Employees { get; set; }
    }
}
