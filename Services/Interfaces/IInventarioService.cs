using practica5PR.Models.ViewModels;

namespace practica5PR.Services.Interfaces
{
    public interface IInventarioService
    {
        Task<InventarioViewModel> ObtenerEstadoInventarioAsync();
    }
}