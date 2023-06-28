using System.ComponentModel.DataAnnotations;

namespace NewsWebsiteFront.Models
{
    public class Authors
    {
        public int Id { get; set; }

        [StringLength(20, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 20 characters.")]
        public string Name { get; set; }
    }
}
