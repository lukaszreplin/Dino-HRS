using Dino.RoomsService.Api.DAL.Entities;

namespace Dino.RoomsService.Api.Services
{
    public interface IRoomService
    {
        Task<IEnumerable<Room>> GetAllAsync();

        Task<Room> GetByIdAsync(Guid id);

        Task Init();
    }
}
