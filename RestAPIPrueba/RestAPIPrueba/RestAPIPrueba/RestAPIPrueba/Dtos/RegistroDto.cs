using System.ComponentModel.DataAnnotations;

namespace RestAPIPrueba.Dtos
{
    public class RegistroDto
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
