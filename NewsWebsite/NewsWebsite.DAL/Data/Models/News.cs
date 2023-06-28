using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebsite.DAL.Data.Models
{
    public class News
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string newsDescription { get; set; }
        public string? image { get; set; }

        [DateValidation(ErrorMessage = "Invalid publication date")]
        public DateTime publicationDate { get; set; }

       // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime creationDate { get; set; } = DateTime.Now;

        [Required]
        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        public Author? Author { get; set; }

    }
}
