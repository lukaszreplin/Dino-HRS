using Dino.RoomsService.Api.DAL;
using Dino.RoomsService.Api.DAL.Entities;
using MongoDB.Driver;

namespace Dino.RoomsService.Api.Services
{
    public class RoomService : IRoomService
    {
        private readonly IMongoCollection<Room> _rooms;
        private readonly ILogger<RoomService> _logger;

        public RoomService(DatabaseSettings settings, ILogger<RoomService> logger, IMongoClient mongoClient)
        {
            _logger = logger;
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _rooms = database.GetCollection<Room>(settings.RoomsCollectionName);
        }

        public async Task<IEnumerable<Room>> GetAllAsync()
        {
            return await _rooms.Find(room => true).ToListAsync();
        }

        public async Task<Room> GetByIdAsync(Guid id)
        {
            return await _rooms.Find(room => room.Id == id).FirstOrDefaultAsync();
        }

        public async Task Init()
        {
            if ((await _rooms.CountDocumentsAsync(room => true)) == 0)
            {
                await _rooms.InsertManyAsync(new[]
                {
                    new Room()
                    {
                        Id = Guid.NewGuid(),
                        Capacity = 2,
                        HasBathroom = true,
                        HasBalcony = true,
                        BedType = "Double",
                        Status = "Active"
                    },
                    new Room()
                    {
                        Id = Guid.NewGuid(),
                        Capacity = 2,
                        HasBathroom = true,
                        HasBalcony = false,
                        BedType = "Twin",
                        Status = "Active"
                    },
                    new Room()
                    {
                        Id = Guid.NewGuid(),
                        Capacity = 1,
                        HasBathroom = false,
                        HasBalcony = true,
                        BedType = "Single",
                        Status = "Active"
                    },
                    new Room()
                    {
                        Id = Guid.NewGuid(),
                        Capacity = 4,
                        HasBathroom = true,
                        HasBalcony = true,
                        BedType = "Twin",
                        Status = "UnderService"
                    }
                });
            }
        }
    }
}
