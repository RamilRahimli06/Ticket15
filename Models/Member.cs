using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ticket15.Models;
public class Member : BaseEntity
{
    [Required]
    [MaxLength(200)]
    public string FullName { get; set; }

    public decimal Salary { get; set; }

    
    public decimal Point { get; set; }

    public string ?ImgUrl { get; set; }

    [NotMapped]
    public IFormFile ?PhotoFile { get; set; }
}