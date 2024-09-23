using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductShopTest.Data.Models
{
    public class Employee
    {
        //one-to-one
        [Key]
        public int EmployeeID { get; set; }

        public string FirstName { get; set; } = null!;

        public int DepartmentId { get; set; }

        [ForeignKey(nameof(Manager))]
        public int? ManagerId { get; set; }
        public virtual Employee? Manager { get; set; }

        public int AddressId { get; set; }
    }
}
