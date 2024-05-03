#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;

namespace Lamborghini.Models;

public class Wedding
{
    [Key]
    public int WeddingId { get; set; }

    [Required(ErrorMessage = "The noble first soul must be named, as they embark on their quest of love.")]
    [MinLength(2, ErrorMessage = "A name should be of worthy length, at least 2 characters.")]
    [MaxLength(244, ErrorMessage = "The name should be a beacon of hope, not exceeding 244 characters.")]
    [Display(Name = "Knight or Lady of the First Realm")]
    public string WedderOne { get; set; }

    [Required(ErrorMessage = "The valiant second soul must be named, as they pledge their allegiance to love's kingdom.")]
    [MinLength(2, ErrorMessage = "A name should be of worthy length, at least 2 characters.")]
    [MaxLength(244, ErrorMessage = "Let the name be a royal decree, not surpassing 244 characters.")]
    [Display(Name = "Knight or Lady of the Second Realm")]
    public string WedderTwo { get; set; }


    [Required(ErrorMessage = "The magical date of the wedding is a vital part of the love story.")]
    [DataType(DataType.Date, ErrorMessage = "The date must be in a valid format.")]
    [PastDate(ErrorMessage = "The date should belong to the annals of history, for it must be in the past.")]
    [Display(Name = "Date of the Royal Union")]
    public DateTime Date { get; set; }


    [Required(ErrorMessage = "The brave heart's abode is a sanctuary of love, and thus required.")]
    [MinLength(2, ErrorMessage = "The address should be of worthy length, at least 2 characters.")]
    [MaxLength(244, ErrorMessage = "The address should not exceed 244 characters, for it is but a humble castle.")]
    [Display(Name = "Castle of the Noble Hearts")]
    public string Address { get; set; }

    //! FK
    public int UserId {get; set;}

    //? Navigation props
    public List<Rsvp> Rsvps { get; set; } = new();
    public User? UserWhoCreatedTheWedding {get; set;}

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdateAt { get; set; } = DateTime.Now;
}

public class PastDateAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value != null && value is DateTime)
        {
            DateTime inputDate = (DateTime)value;
            DateTime currentDate = DateTime.Now;

            if (inputDate < currentDate)
            {
                return new ValidationResult("The date must be in the past.");
            }
        }

        return ValidationResult.Success;
    }
}
