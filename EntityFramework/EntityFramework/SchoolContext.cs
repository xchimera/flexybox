using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFramework
{
    class SchoolContext : DbContext
    {
        public SchoolContext()
            : base("ConnectionString")
        {
            Database.SetInitializer<SchoolContext>(new DropCreateDatabaseAlways<SchoolContext>());
        }
        /// <summary>
        /// Use to add a new entity or update an existing entity
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="entity">Entity to save</param>
        /// <returns></returns>
        public bool SaveEntity <T> (T entity)  where T : class
        {
            if (entity == null)
                return false;

            if ((entity as Entity).Id == 0)
            {
                Entry(entity).State = EntityState.Added;
                Set(typeof(T)).Add(entity);
            }
            else if ((entity as Entity).Id > 0)
                Entry(entity).State = EntityState.Modified;            
            if (SaveChanges() > 0)
                return true;
            return false;
        }

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
                if ((entity as Entity).Id == 0)
                {
                    Set(typeof(T)).Add(entity);
                    Entry((entity as Entity)).State = EntityState.Added;
                }
                else
                    Entry((entity as Entity)).State = EntityState.Modified;
            }
            if(SaveChanges() > 0)
                return true;
            return false;
        }

        //public bool SaveEntities<T>(IEnumerable<T> entities)
        //{
        //    if (entities == null)
        //        return false;
        //    foreach (var entity in entities)
        //    {
        //        if ((entity as Student).Id == 0)
        //        {
        //            Set(typeof(T)).Add(entity);
        //            Entry((entity as Student)).State = EntityState.Added;
        //        }
        //        else
        //            Entry((entity as Student)).State = EntityState.Modified;
        //    }
        //    var a = SaveChanges();
        //    return true;
        //}

        /// <summary>
        /// Deletes a single entity
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="entity">Entity to delete</param>
        /// <returns></returns>
        public bool DeleteEntity<T> (T entity) where T : class
        {
            if (entity == null)
                return false;
            Entry(entity).State = EntityState.Deleted;
            if (SaveChanges() > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Deletes a IEnumerable list of entities
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="entity">IEnumerable list of entities to delete</param>
        /// <returns></returns>
        public bool DeleteEntities<T> (IEnumerable<T> entities) where T : class
        {
            if (entities == null)
                return false;

            foreach(var entity in entities)
            {
                Entry(entity).State = EntityState.Deleted;
            }
            if (SaveChanges() > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Get an IQueryable of the selected type
        /// </summary>
        /// <typeparam name="T">Type to find</typeparam>
        /// <returns>IQueryable of T</returns>
        public IQueryable<T> Query<T> () where T : class
        {
            return Set<T>();
        }

        //public T Query<T>(int key)
        //{

        //}



        //public DbSet<Student> Students { get; set; }
        //public DbSet<Standard> Standards { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var entitymethod = typeof(DbModelBuilder).GetMethod("Entity");

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var entitytypes = assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(Entity)));
                foreach (var type in entitytypes)
                {
                    entitymethod.MakeGenericMethod(type).Invoke(modelBuilder, new object[] { });
                }
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
