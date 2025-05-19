using Dino.PaymentsService.Api.DAL;
using Dino.PaymentsService.Api.DAL.Entities;
using Dino.PaymentsService.Api.Dto;
using MongoDB.Driver;

namespace Dino.PaymentsService.Api.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IMongoCollection<Payment> _payments;

        public PaymentService(DatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _payments = database.GetCollection<Payment>(settings.PaymentsCollectionName);
        }

        public async Task<IEnumerable<Payment>> GetAll()
        {
            return await _payments.Find(payment => true).ToListAsync();
        }

        public async Task<Payment> GetByIdAsync(Guid id)
        {
            return await _payments.Find(payment => payment.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Payment>> GetByReservationIdAsync(Guid reservationId)
        {
            return await _payments.Find(payment => payment.ReservationId == reservationId).ToListAsync();
        }

        public async Task<Payment> ProcessPaymentAsync(ProcessPaymentRequest request)
        {
            var now = DateTime.UtcNow;

            var payment = new Payment
            {
                ReservationId = request.ReservationId,
                Amount = request.Amount,
                Currency = request.Currency,
                PaymentMethod = request.PaymentMethod,
                Status = "Processing",
                Date = now,
                LastUpdated = now
            };

            await _payments.InsertOneAsync(payment);

            return payment;
        }

        public async Task<Payment> UpdatePaymentStatusAsync(Guid id, string status)
        {
            var filter = Builders<Payment>.Filter.Eq(p => p.Id, id);
            var update = Builders<Payment>.Update
                .Set(p => p.Status, status)
                .Set(p => p.LastUpdated, DateTime.UtcNow);

            await _payments.UpdateOneAsync(filter, update);

            return await GetByIdAsync(id);
        }
    }
}
