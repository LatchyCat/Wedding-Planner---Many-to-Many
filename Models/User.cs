#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Lamborghini.Models;


public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "The brave knight's first name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The noble princess's last name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The herald must be provided with a valid email.")]
        // [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "The provided email must be of a valid format.")]
        [EmailAddress(ErrorMessage = "The provided email must be of a valid format.")]
        [UniqueEmail(ErrorMessage = "The email must be unique across the realm.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The secret code to access the knight's chamber is required.")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "The password must be at least 8 characters long.")]
        public string Password { get; set; }

        [NotMapped]
        [Compare("Password", ErrorMessage = "The confirmation of the secret code does not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        //? Navigation props
        public List<Rsvp> Rsvps { get; set; } = new();

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }


public class UniqueEmailAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
    	// Though we have Required as a validation, sometimes we make it here anyways
    	// In which case we must first verify the value is not null before we proceed
        if(value == null)
        {
    	    // If it was, return the required error
            return new ValidationResult("Email is required!");
        }

    	// This will connect us to our database since we are not in our Controller
        MyContext _context = (MyContext)validationContext.GetService(typeof(MyContext));
        // Check to see if there are any records of this email in our database
    	if(_context.Users.Any(e => e.Email == value.ToString()))
        {
    	    // If yes, throw an error
            return new ValidationResult("The email must be unique across the realm.");
        } else {
    	    // If no, proceed
            return ValidationResult.Success;
        }
    }
}
