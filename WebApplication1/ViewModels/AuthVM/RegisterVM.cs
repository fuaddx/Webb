using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace WebApplication1.ViewModel.AuthVM 
{
    public class RegisterVM
    {
		[Required(ErrorMessage = "Enter Name!"), MaxLength(36)]
		public string Fisrtname { get; set; }
		[Required(ErrorMessage = "Enter Surname!"), MaxLength(36)]
		public string Lastname { get; set; }
		[Required, DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		[Required(ErrorMessage = "Enter Username Correctly!"), MaxLength(24)]
		public string Username { get; set; }
		[Required, DataType(DataType.Password), Compare(nameof(ConfirmPassword)), RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{4,}$", ErrorMessage = "Wrong input for password")]
		public string Password { get; set; }
		[Required, DataType(DataType.Password), RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{4,}$", ErrorMessage = "Wrong input for password")]
		public string ConfirmPassword { get; set; }
	}
}
