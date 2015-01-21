using System;
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
        //lavet af Søren
        /// <summary>
        /// Get an IQueryable of the selected type
        /// </summary>
        /// <typeparam name="T">Type to find</typeparam>
        /// <returns>IQueryable of T</returns>
        public IQueryable<T> Query<T>(bool includeDeleted = false) where T : class
        {
            //tjek typen der skal hente er en entitet og om man skal hente slettede med
            if (!includeDeleted && typeof(T).IsSubclassOf(typeof(EntityPersist)))
            {
                //hent alle ud hvor IsDeleted er false eller 0 i databasen
                return Set<T>().Where("IsDeleted = @0", false);
            }
            //returner alle, også dem som er slettet
            return Set<T>();
        }

        //lavet af Søren
        public IQueryable<T> QueryFromID<T>(int Id) where T : class
        {
            //tjek typen der skal hente er en entitet 
            if (typeof(T).IsSubclassOf(typeof(EntityPersist)))
            {
                //hent en entitet med et specifikt Id
                return Set<T>().Where("Id = " + Id, false);
            }
            //kast en exception hvis T ikke arver fra EntityPersist
            throw new NotSupportedException("QueryFromID must be called with T that inherits from EntityPersist");
        }

        //lavet af Søren 
        /// <summary>
        /// Deletes a IEnumerable list of entities
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="entity">IEnumerable list of entities to delete</param>
        /// <returns></returns>
        public bool DeleteEntities<T>(IEnumerable<T> entities) where T : class
        {
            //hvis listen der skal gemmes er null, gå ud af metoden og returner false
            if (entities == null)
                return false;

            foreach (var entity in entities)
            {
                //tjek om T arver fra EntityPersist
                if (entity.GetType().IsAssignableFrom(typeof(EntityPersist)))
                {
                    //vær sikker på at entiteterne bliver opdateret og ikke gemt som nye
                    Entry(entity).State = EntityState.Modified;
                    //sæt at entiteten er slettet
                    (entity as EntityPersist).IsDeleted = true;
                }
                else
                    //sæt at entiteten er slettet
                    Entry(entity).State = EntityState.Deleted;
            }
            if (SaveChanges() > 0)
                return true;
            return false;
        }

        //lavet af Søren 
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
                //vær sikker på at entiteterne bliver opdateret og ikke gemt som nye
                Entry(entity).State = EntityState.Modified;
                //sæt at entiteten er slettet

                (entity as EntityPersist).IsDeleted = true;
            }
            else
                //sæt at entiteten er slettet
                Entry(entity).State = EntityState.Deleted;
            if (SaveChanges() > 0)
                return true;
            return false;
        }

        //lavet af Søren 
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
                //hvis Idet er 0 skal entiteten tilføjes som en ny
                Entry(entity).State = EntityState.Added;
                //tilføj entiteten til listen af dens type
                Set(typeof(T)).Add(entity);
            }
            else if ((entity as EntityPersist).Id > 0)
                //hvis entitetens Id er højere end 0 findes den allerede og den skal opdateres
                Entry(entity).State = EntityState.Modified;
            //gem alle ændringer og returner true hvis der ingen fejl var eller false hvis der var fejl
            if (SaveChanges() > 0)
                return true;
            return false;
        }

        //lavet af Søren 
        /// <summary>
        /// Use to add a new collection of entities or update an existing (collection of) entity
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="entity">Entities to save</param>
        /// <returns>bool</returns>
        public bool SaveEntities<T>(IEnumerable<T> entities)
        {
            if (entities == null)
                return false;
            foreach (var entity in entities)
            {
                if ((entity as EntityPersist).Id == 0)
                {
                    //hvis Idet er 0 skal entiteten tilføjes som en ny
                    Set(typeof(T)).Add(entity);
                    Entry((entity as EntityPersist)).State = EntityState.Added;
                }
                else
                    //hvis entitetens Id er højere end 0 findes den allerede og den skal opdateres
                    Entry((entity as EntityPersist)).State = EntityState.Modified;
            }
            //gem alle ændringer og returner true hvis der ingen fejl var eller false hvis der var fejl
            if (SaveChanges() > 0)
                return true;
            return false;
        }

        //lavet af Søren 
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //find Entity metoden på DbModelBuilder via reflection
            var entitymethod = typeof(DbModelBuilder).GetMethod("Entity");

            //find alle assemblies tilknyttet til den programmet
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                //find alle de klasser der arver fra EntityPersist
                var entitytypes = assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(EntityPersist)));
                foreach (var type in entitytypes)
                {
                    //lav et DbSet af hver type
                    entitymethod.MakeGenericMethod(type).Invoke(modelBuilder, new object[] { });
                }
            }
            

            base.OnModelCreating(modelBuilder);
        }
    }
}
