using System.Collections.Generic;

namespace practica5PR.Models.ViewModels
{
    public class ReporteViewModel
    {
        public string Titulo { get; set; } = string.Empty;
        public List<Medicamento> Medicamentos { get; set; } = new();
        public DateTime FechaGeneracion { get; set; } = DateTime.Now;
    }
}
