using System.ComponentModel.DataAnnotations;

namespace Curd.Common.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
    }
}
