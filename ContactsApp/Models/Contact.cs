using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ContactsApp.Models
{
    public class Contact
    {
        public int Id { get; set; }
        [Required] public string FirstName { get; set; } = "";
        [Required] public string LastName { get; set; } = "";

        [Required, EmailAddress]
        public string Email { get; set; } = ""; // unique in db

        [Required, MinLength(8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "Password must contain big and small letters as well as a digit.")]
        public string Password { get; set; } = "";

        [Required] public string Category { get; set; } = ""; // work, private, other
        public string? Subcategory { get; set; }
        [Required] public string PhoneNumber { get; set; }
        [Required] public DateTime BirthDate { get; set; }
    }
}
