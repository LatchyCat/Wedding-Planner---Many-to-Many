#pragma warning disable CS8618
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lamborghini.Models
{
    public class Rsvp
    {
        [Key]
        public int RsvpId { get; set; }

        //! Foreign key
        [Required(ErrorMessage = "User ID is required.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Wedding ID is required.")]
        public int WeddingId { get; set; }

        //! Navigation properties
        public User? Guest { get; set; }
        public Wedding? Wedding { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
