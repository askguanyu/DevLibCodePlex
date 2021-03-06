﻿//-----------------------------------------------------------------------
// <copyright file="IRepository.cs" company="YuGuan Corporation">
//     Copyright (c) YuGuan Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace DevLib.Data
{
    using System.Collections.Generic;

    /// <summary>
    /// Generic repository interface for reading and writing domain entities to a storage.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity.</typeparam>
    /// <typeparam name="TPrimaryKey">Type of the primary key.</typeparam>
    public interface IRepository<TEntity, TPrimaryKey> where TEntity : class
    {
        /// <summary>
        /// Inserts entity to the storage.
        /// </summary>
        /// <param name="entity">Entity instance.</param>
        /// <returns>true if succeeded; otherwise, false.</returns>
        bool Insert(TEntity entity);

        /// <summary>
        /// Updates entity in the storage.
        /// </summary>
        /// <param name="entity">Entity instance.</param>
        /// <returns>true if succeeded; otherwise, false.</returns>
        bool Update(TEntity entity);

        /// <summary>
        /// Deletes entity in the storage.
        /// </summary>
        /// <param name="id">Entity id.</param>
        /// <returns>true if succeeded; otherwise, false.</returns>
        bool DeleteById(TPrimaryKey id);

        /// <summary>
        /// Deletes entity in the storage.
        /// </summary>
        /// <param name="entity">Entity instance.</param>
        /// <returns>true if succeeded; otherwise, false.</returns>
        bool Delete(TEntity entity);

        /// <summary>
        /// Determines whether the specified entity exists.
        /// </summary>
        /// <param name="id">Entity id.</param>
        /// <returns>true if the specified entity exists; otherwise, false.</returns>
        bool ExistsById(TPrimaryKey id);

        /// <summary>
        /// Determines whether the specified entity exists.
        /// </summary>
        /// <param name="entity">Entity instance.</param>
        /// <returns>true if the specified entity exists; otherwise, false.</returns>
        bool Exists(TEntity entity);

        /// <summary>
        /// Gets entity from the storage by entity id.
        /// </summary>
        /// <param name="id">Entity id.</param>
        /// <returns>Entity instance.</returns>
        TEntity GetById(TPrimaryKey id);

        /// <summary>
        /// Gets all entities of the type from the storage.
        /// </summary>
        /// <returns>List of entity instance.</returns>
        IList<TEntity> GetAll();
    }
}
