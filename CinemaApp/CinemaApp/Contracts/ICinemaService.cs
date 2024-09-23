using CinemaApp.Models;

namespace CinemaApp.Contracts
{
    public interface ICinemaService
    {
        Task AddCinemaAsync(CinemaModel model);
    }
}
