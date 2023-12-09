using MongoDB.Driver;
using MongoRM.Data.Interfaces;
using MongoRM.DataAccess.DAL;
using MongoRM.DataAccess.Exceptions;
using MongoRM.DataAccess.Interfaces;

namespace MongoRM.DataAccess.Abstractions;

/**
 * <typeparam name="TEntity">An IEntity that represents data in the MongoDb. This should be a concrete class.</typeparam>
 * <summary>
 * AbstractEntityService implements basic read / write procedures to access IEntity objects stored in MongoDB.
 * This abstract class should be inherited by a concrete class, where the concrete class defines the generic type represented by the IEntity.
 * The concrete class then has access to the standard read/write procedures and can overwrite them or add additional procedures.
 * For example: BookEntityService : AbstractEntityService&lt;Book&gt;
 * </summary>
 */
public class AbstractEntityService<TEntity> : IEntityService<TEntity> where TEntity : IEntity
{
    protected readonly IMongoCollection<TEntity> Collection;
    
    /**
     * <param name="mongoDb">An instance of a that contains a database connection</param>
     * <summary>Instantiate a new AbstractEntityService using the IMongoDb interface</summary>
     */
    public AbstractEntityService(IMongoDb mongoDb)
    {
        Collection = mongoDb.Database.GetCollection<TEntity>(typeof(TEntity).Name);
    }
    
    /**
     * <inheritdoc/>
     * <exception cref="MongoRmWarningException">Operation failed due to a known underlying circumstance as outlined in the message.</exception>
     * <exception cref="MongoRmCriticalException">Operation failed due to an unknown circumstance.</exception>
     */
    public async Task<TEntity> Get(string id)
    {
        var result = await Collection.FindAsync<TEntity>(x => id == x.Id, default, default);
        if (result is not null)
        {
            var x = result.Current;
            try
            {
                return await result.FirstAsync();
            }
            catch (Exception)
            {
                throw new MongoRmWarningException($"Could not find {typeof(TEntity).Name} with Id {id}");
            }
        }
        throw new MongoRmWarningException($"Could not find {typeof(TEntity).Name} with Id {id}");
    }
    
    /**
     * <inheritdoc/>
     * <exception cref="MongoRmWarningException">Operation failed due to a known underlying circumstance as outlined in the message.</exception>
     * <exception cref="MongoRmCriticalException">Operation failed due to an unknown circumstance.</exception>
     */
    public async Task<TEntity> Create(TEntity entity)
    {
        try
        {
            await Collection.InsertOneAsync(entity);
        }
        catch (Exception)
        {
            throw new MongoRmCriticalException($"{typeof(TEntity).Name} creation failed!");
        }
        var result = await Collection.FindAsync(x => entity.Id == x.Id);
        if (result is not null)
        {
            try
            {
                return await result.FirstAsync();
            }
            catch (Exception)
            {
                throw new MongoRmWarningException($"{typeof(TEntity).Name} save successful but does not appear in the database!");
            }
        } 
        throw new MongoRmWarningException($"{typeof(TEntity).Name} save successful but does not appear in the database!");
    }

    /**
     * <inheritdoc/>
     * <exception cref="MongoRmWarningException">Operation failed due to a known underlying circumstance as outlined in the message.</exception>
     * <exception cref="MongoRmCriticalException">Operation failed due to an unknown circumstance.</exception>
     */
    public async Task<TEntity> Update(TEntity entity)
    {
        try
        {
            await Collection.ReplaceOneAsync(x => entity.Id == x.Id, entity);
        }
        catch (Exception)
        {
            throw new MongoRmWarningException($"{typeof(TEntity).Name} update not successful!");
        }
        var result = await Collection.FindAsync(x => entity.Id == x.Id);
        if (result is not null)
        {
            try
            {
                return await result.FirstAsync();
            }
            catch (Exception)
            {
                throw new MongoRmWarningException($"{typeof(TEntity).Name} update successful but does not appear in the database!");
            }
        }
        throw new MongoRmWarningException($"{typeof(TEntity).Name} update successful but does not appear in the database!");
    }

    /**
     * <inheritdoc/>
     * <exception cref="MongoRmWarningException">Operation failed due to a known underlying circumstance as outlined in the message.</exception>
     * <exception cref="MongoRmCriticalException">Operation failed due to an unknown circumstance.</exception>
     */
    public async Task<TEntity> Delete(string id)
    {
        var result = await Collection.FindAsync(x => id == x.Id);
        if (result is null) throw new MongoRmWarningException($"{typeof(TEntity).Name} with id {id} not found!");
        try
        {
            await Collection.DeleteOneAsync(x => id == x.Id);
        }
        catch (Exception)
        {
            throw new MongoRmWarningException($"{typeof(TEntity).Name} delete not successful!");
        }
        try
        {
            return await result.FirstAsync();
        }
        catch (Exception)
        {
            throw new MongoRmWarningException($"{typeof(TEntity).Name} delete successful but does not appear in the database!");
        }
    }
}