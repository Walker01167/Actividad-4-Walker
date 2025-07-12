using System.ComponentModel.DataAnnotations;

namespace RestAPIPrueba.Models
{
    public class Proveedor
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "El proveedor no puede estar vacio.")]
        public string NombreProveedor { get; set; } = null!;

        [Phone(ErrorMessage = "Introduce un numero de telefono valido.")]
        public string Telefono { get; set; } = null!;

        [EmailAddress(ErrorMessage = "Proporciona un correo electronico valido.")]
        public string Correo { get; set; } = null!;
    }
}
