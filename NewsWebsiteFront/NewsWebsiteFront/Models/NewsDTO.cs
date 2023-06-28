using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
namespace NewsWebsiteFront.Models
{
    public class NewsDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a value for the field.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please enter a value for the field.")]
        public string newsDescription { get; set; }
        [Required]
        public IFormFile? imageURL { get; set; }
        public string? image { get; set; }
        [Required]
        [DateValidation]
        public DateTime publicationDate { get; set; }
        public DateTime creationDate { get; set; } = DateTime.Now;
        [Required]
        public int AuthorId { get; set; }
    }
}
