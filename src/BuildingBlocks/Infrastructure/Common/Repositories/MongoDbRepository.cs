using Contracts.Domains;
using Contracts.Domains.Interfaces;
using Infrastructure.Extensions;
using MongoDB.Driver;
using Shared.Configurations;

namespace Infrastructure.Common.Repositories
{
    public class MongoDbRepository<T> : IMongoDbRepositoryBase<T> where T : MongoEntity
    {
        public IMongoDatabase Database { get; }

        public MongoDbRepository(IMongoClient client, MongoDbSettings settings)
        {
            Database = client.GetDatabase(settings.DatabaseName);
        }
        protected virtual IMongoCollection<T> Collection => Database.GetCollection<T>(GetCollectionName());

        public Task CreateAsync(T entity)
        {
            return Collection.InsertOneAsync(entity);
        }

        public Task DeleteAsync(string id)
        {
            return Collection.DeleteOneAsync(x => x.Id.Equals(id));
        }

        public IMongoCollection<T> FindAll(ReadPreference? readPreference = null)
        {
            return Database.WithReadPreference(readPreference ?? ReadPreference.Primary).GetCollection<T>(GetCollectionName());
        }

        public Task UpdateAsync(T entity)
        {
            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty == null)
                throw new InvalidOperationException("Entity does not contain an Id property.");

            string? value = idProperty.GetValue(entity) as string;
            var filter = Builders<T>.Filter.Eq("Id", value);

            return Collection.ReplaceOneAsync(filter, entity);
        }

        private static string? GetCollectionName()
        {
            return (typeof(T).GetCustomAttributes(typeof(BsonCollectionAttribute), true).FirstOrDefault() as
                BsonCollectionAttribute)?.CollectionName;
        }
    }
}
