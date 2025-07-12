using System;

using System.ComponentModel.DataAnnotations;

namespace RestAPIPrueba.Models

{

    public class Inventario

    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Debes colocar el productoID")]

        public int ProductoId { get; set; }

        [Required]

        [Range(1, int.MaxValue, ErrorMessage = "No puede ser 0")]

        public int Cantidad { get; set; }

        [Required]

        [DataType(DataType.Date)]

        [CustomValidation(typeof(Inventario), nameof(ValidarFechaEntrada))]

        public DateTime FechaEntrada { get; set; }

        public static ValidationResult? ValidarFechaEntrada(DateTime fecha, ValidationContext context)

        {

            if (fecha > DateTime.Now)

                return new ValidationResult("La fecha debe ser correcta.");

            return ValidationResult.Success;

        }

    }

}
