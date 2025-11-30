using System.ComponentModel.DataAnnotations;

namespace api.Models.Data
{
    public class UserLogin
    {
        [Required]
        [StringLength(100)]
        public string? Username { get; set; }
        [Required]
        [StringLength(150)]
        public string? Password { get; set; }
    }
}
