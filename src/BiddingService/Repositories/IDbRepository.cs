using MongoDB.Entities;

namespace BiddingService.Repositories;

public interface IDbRepository<T> where T : Entity
{
    Task InsertAsync(T entity);
}