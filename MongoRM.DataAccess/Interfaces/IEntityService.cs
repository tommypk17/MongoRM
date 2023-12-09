﻿namespace MongoRM.DataAccess.Interfaces;

public interface IEntityService<TEntity>
{
    /**
     * <param name="id">unique identifier of the entity in the collection</param>
     * <typeparam name="TEntity">An IEntity that represents data in the MongoDb. This should be a concrete class.</typeparam>
     * <summary>Get an entity from the MongoDb Collection using the Id of the entity.</summary>
     * <returns>Async token containing the TEntity of the found entity.</returns>
     */
    public Task<TEntity> Get(string id);
    
    /**
     * <param name="entity">An IEntity to be created in the MongoDb</param>
     * <typeparam name="TEntity">An IEntity that represents data in the MongoDb. This should be a concrete class.</typeparam>
     * <summary>Insert an entity into the MongoDb Collection.</summary>
     * <returns>Async token containing the TEntity of the created entity.</returns>
     */
    public Task<TEntity> Create(TEntity entity);
    
    /**
     * <param name="entity">An IEntity to be created in the MongoDb</param>
     * <typeparam name="TEntity">An IEntity that represents data in the MongoDb. This should be a concrete class.</typeparam>
     * <summary>Update an existing entity in the MongoDb Collection.</summary>
     * <returns>Async token containing the TEntity of the updated entity.</returns>
     */
    public Task<TEntity> Update(TEntity entity);
    
    /**
     * <param name="id">unique identifier of the entity in the collection</param>
     * <typeparam name="TEntity">An IEntity that represents data in the MongoDb. This should be a concrete class.</typeparam>
     * <summary>Delete an existing entity from the MongoDb Collection.</summary>
     * <returns>Async token containing the TEntity of the deleted entity.</returns>
     */
    public Task<TEntity> Delete(string id);
}
