using Microsoft.AspNetCore.Identity;
namespace Product_Inventory_System.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
