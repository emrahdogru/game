using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using War.Server.Database;

namespace War.Server.Domain.Repositories
{
    public abstract class Repository<T> where T : Entity
    {

        private static IMongoClient? _client = null;
        private static IMongoDatabase? _database = null;

        public static IMongoClient GetClient()
        {
            _client ??= new MongoClient("mongodb://localhost:27017");
            return _client;
        }

        public static IMongoDatabase GetDatabase()
        {
            _database ??= GetClient().GetDatabase("WarGame");
            return _database;
        }

        public static IMongoCollection<T> GetCollection()
        {
            var dbAttribute = typeof(T).GetCustomAttribute<DbAttribute>(true)
                ?? throw new NullReferenceException($"There is no DbAttribute for type `{typeof(T).FullName}`");

            return GetDatabase().GetCollection<T>(dbAttribute.CollectionName);
        }


        /// <summary>
        /// Kayıt veritabanında varsa günceller, yoksa ekler
        /// </summary>
        /// <param name="entity"></param>
        /// <exception cref="Exception"></exception>
        public static async Task SaveAsync(T entity, IClientSessionHandle? sessionHandle = null) 
        {
            var replaceOptions = new ReplaceOptions() { IsUpsert = true };
            try
            {
                if (sessionHandle != null)
                    await GetCollection().ReplaceOneAsync(sessionHandle, x => x.Id == entity.Id, entity, replaceOptions);
                else
                    await GetCollection().ReplaceOneAsync(x => x.Id == entity.Id, entity, replaceOptions);
            }
            catch (MongoWriteException ex)
            {

                //A bulk write operation resulted in one or more errors.
                // E11000 duplicate key error index: bastapp.User.$Email dup key: { : "emrahdogru@gmail.com", : ObjectId('57d830d40f42fa2bd8ecad4a') }
                if (ex.WriteError.Code == 11000)
                {
                    throw new DuplicateKeyException(ex.Message, ex);
                }
                else
                {
                    throw;
                }
            }
        }

        public static IQueryable<T> GetAll()
        {
            return GetCollection().AsQueryable();
        }

        public static T? Find(ObjectId id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);
        }

        public static T FindOrThrow(ObjectId id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id) ?? throw new EntityNotFountException(typeof(T).Name, id);
        }

        public static void Delete(T entity)
        {
            GetCollection().DeleteOne(x  => x.Id == entity.Id);
        }

        public static void DeleteMany(IEnumerable<T> entities)
        {
            var ids = entities.Select(x => x.Id).ToArray();
            GetCollection().DeleteMany(x => ids.Contains(x.Id));
        }

        public static void DeleteMany(Expression<Func<T, bool>> filter)
        {
            GetCollection().DeleteMany(filter);
        }

        public static bool IsNew(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            return GetAll().Any(x => x.Id == entity.Id);
        }
    }
}
