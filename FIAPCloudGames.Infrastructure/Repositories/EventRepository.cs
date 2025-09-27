using FIAPCloudGames.Domain.Entities;
using FIAPCloudGames.Domain.Interfaces;
// A referência ao LiteDB pode ser removida
// using LiteDB; 

namespace FIAPCloudGames.Infrastructure.Repositories
{
    public class EventRepository : IEventRepository
    {
        public EventRepository()
        {
        }

        public Task<IEnumerable<DomainEvent>> GetAll(int skip, int take)
        {
            return Task.FromResult(new List<DomainEvent>().AsEnumerable());
        }

        public Task<IEnumerable<DomainEvent>> GetAllByAggregateId(Guid aggregateId)
        {
            return Task.FromResult(new List<DomainEvent>().AsEnumerable());
        }

        public Task Save(DomainEvent domainEvent)
        {
            Console.WriteLine($"[DEBUG] Event Sourcing está desativado. Evento '{domainEvent.EventType}' não foi guardado.");
            return Task.CompletedTask;
        }
    }
}