namespace practica5PR.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalMedicamentos { get; set; }
        public int MedicamentosVencidos { get; set; }
        public int MedicamentosPorVencer { get; set; }
        public int MedicamentosBajoStock { get; set; }
        public int TotalCategorias { get; set; }
        public int TotalEstantes { get; set; }
    }
}