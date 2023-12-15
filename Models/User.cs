using Microsoft.AspNetCore.Identity;

namespace Marketplace.Models;

public class User : IdentityUser<int>
{
    public decimal Money { get; set; }
}
