using System.ComponentModel.DataAnnotations;

namespace Ticket15.Models
{
    public class Agent : BaseEntity
    {

        [Required]
        [MaxLength(100,ErrorMessage="FullName uzunlugu 100 simvoldan cox ola bilmez")]
        [MinLength(1, ErrorMessage = "FullName uzunlugu 1 simvoldan az ola bilmez")]
        public string FullName { get; set; }
        [Required]
        [MaxLength(100,ErrorMessage="Designation uzunlugu 100 simvoldan cox ola bilmez")]
        [MinLength(1, ErrorMessage = "Designation uzunlugu 1 simvoldan az ola bilmez")]
        public string Designation { get; set; }

        public string ImgUrl { get; set; }


    }
}
