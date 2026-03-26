using practica5PR.Models;

namespace practica5PR.Models.ViewModels
{
    public class InventarioViewModel
    {
        public List<Medicamento> Vencidos { get; set; } = new();
        public List<Medicamento> PorVencer30Dias { get; set; } = new();
        public List<Medicamento> PorVencer60Dias { get; set; } = new();
        public List<Medicamento> PorVencer90Dias { get; set; } = new();
        public List<Medicamento> BajoStock { get; set; } = new();
    }
}