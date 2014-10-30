﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlexyDomain.Models;
using FlexyDomain.Extensions;

namespace FlexyDomain
{
    public class FlexyboxContext : DbContext
    {
        public FlexyboxContext()
            : base("ConnectionString")
        {
        }
        //lavet af Søren Pedersen
        /// <summary>
        /// Get an IQueryable of the selected type
        /// </summary>
        /// <typeparam name="T">Type to find</typeparam>
        /// <returns>IQueryable of T</returns>
        public IQueryable<T> Query<T>(bool includeDeleted) where T : class
        {
            if (!includeDeleted && typeof(T).IsSubclassOf(typeof(EntityPersist)))
            {
                return Set<T>().Where("IsDeleted = @0", false);
            }
            return Set<T>();
        }

        //lavet af Søren Pedersen
        public IQueryable<T> QueryFromID<T>(int Id) where T : class
        {
            if (typeof(T).IsSubclassOf(typeof(EntityPersist)))
            {
                return Set<T>().Where("Id = " + Id, false);
            }
            throw new NotSupportedException("QueryFromID must be called with T that inherits from EntityPersist");
        }

        //lavet af Søren Pedersen
        /// <summary>
        /// Deletes a IEnumerable list of entities
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="entity">IEnumerable list of entities to delete</param>
        /// <returns></returns>
        public bool DeleteEntities<T>(IEnumerable<T> entities) where T : class
        {
            if (entities == null)
                return false;

            foreach (var entity in entities)
            {
                if (entity.GetType().IsAssignableFrom(typeof(EntityPersist)))
                {
                    Entry(entity).State = EntityState.Modified;
                    (entity as EntityPersist).IsDeleted = true;
                }
                else
                    Entry(entity).State = EntityState.Deleted;
            }
            if (SaveChanges() > 0)
                return true;
            return false;
        }

        //lavet af Søren Pedersen
        /// <summary>
        /// Deletes a single entity
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="entity">Entity to delete</param>
        /// <returns></returns>
        public bool DeleteEntity<T>(T entity) where T : class
        {
            if (entity == null)
                return false;
            if (entity is EntityPersist)
            {
                Entry(entity).State = EntityState.Modified;
                (entity as EntityPersist).IsDeleted = true;
            }
            else
                Entry(entity).State = EntityState.Deleted;
            if (SaveChanges() > 0)
                return true;
            return false;
        }

        //lavet af Søren Pedersen
        /// <summary>
        /// Use to add a new entity or update an existing entity
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="entity">Entity to save</param>
        /// <returns></returns>
        public bool SaveEntity<T>(T entity) where T : class
        {
            if (entity == null)
                return false;

            if ((entity as EntityPersist).Id == 0)
            {
                Entry(entity).State = EntityState.Added;
                Set(typeof(T)).Add(entity);
            }
            else if ((entity as EntityPersist).Id > 0)
                Entry(entity).State = EntityState.Modified;
            if (SaveChanges() > 0)
                return true;
            return false;
        }

        //lavet af Søren Pedersen
        /// <summary>
        /// Use to add a new collection of entities or update an existing (collection of) entity
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="entity">Entities to save</param>
        /// <returns></returns>

        public bool SaveEntities<T>(IEnumerable<T> entities)
        {
            if (entities == null)
                return false;
            foreach (var entity in entities)
            {
                if ((entity as EntityPersist).Id == 0)
                {
                    Set(typeof(T)).Add(entity);
                    Entry((entity as EntityPersist)).State = EntityState.Added;
                }
                else
                    Entry((entity as EntityPersist)).State = EntityState.Modified;
            }
            if (SaveChanges() > 0)
                return true;
            return false;
        }

        //lavet af Søren Pedersen
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var entitymethod = typeof(DbModelBuilder).GetMethod("Entity");

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var entitytypes = assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(EntityPersist)));
                foreach (var type in entitytypes)
                {
                    entitymethod.MakeGenericMethod(type).Invoke(modelBuilder, new object[] { });
                }
            }
            base.OnModelCreating(modelBuilder);
        }
    }
}
