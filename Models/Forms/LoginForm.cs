using System.ComponentModel.DataAnnotations;

namespace Marketplace.Models.Forms;

public class LoginForm
{
	[Required]
	[EmailAddress]
	public string Login { get; set; }
	[Required]
	public string Password { get; set; }
}
