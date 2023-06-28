using NewsWebsite.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace NewsWebsite.BL.DTO
{
    public class NewsDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string newsDescription { get; set; }
        public string? image { get; set; }
        //public string? imageURL { get; set; }

        [DateValidation(ErrorMessage = "Invalid publication date")]
        public DateTime publicationDate { get; set; }  
        public DateTime creationDate { get; set; } = DateTime.Now;  
        public int AuthorId { get; set; }
    }
}
