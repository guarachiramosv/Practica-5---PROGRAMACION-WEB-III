using System.ComponentModel.DataAnnotations;

namespace practica5PR.Models
{
    public class Estante
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del estante es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(150, ErrorMessage = "La ubicación no puede superar los 150 caracteres.")]
        [Display(Name = "Ubicación")]
        public string? Ubicacion { get; set; }

        [StringLength(250, ErrorMessage = "La descripción no puede superar los 250 caracteres.")]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        public ICollection<Medicamento> Medicamentos { get; set; } = new List<Medicamento>();
    }
}