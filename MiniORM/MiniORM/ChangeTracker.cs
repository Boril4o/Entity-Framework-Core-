using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace MiniORM
{
    internal class ChangeTracker<T>
    where T : class, new()
    {
       private readonly IList<T> allEntities;

       private readonly IList<T> added;

       private readonly IList<T> removed;

       public ChangeTracker(IEnumerable<T> entities)
       {
            added = new List<T>();
            removed = new List<T>();

            allEntities = ClonedEntities(entities);
       }

       public IReadOnlyCollection<T> AllEntities 
           => (IReadOnlyCollection<T>)allEntities;

       public IReadOnlyCollection<T> Added
       => (IReadOnlyCollection<T>)added;

       public IReadOnlyCollection<T> Removed
       => (IReadOnlyCollection<T>)removed;

       public void Add(T item) 
           => added.Add(item);

       public void Remove(T item) 
           => removed.Add(item);

       public IEnumerable<T> GetModifiedEntities(DbSet<T> dbSet)
       {
            IList<T> modifiedEntities = new List<T>();

            PropertyInfo[] primaryKeys = typeof(T)
                .GetProperties()
                .Where(pi => pi.HasAttribute<KeyAttribute>())
                .ToArray();

            foreach (var proxyEntity in this.AllEntities)
            {
                var primaryKayValues = GetPrimaryKeyValues(primaryKeys, proxyEntity).ToArray();

                var entity = dbSet.Entities.Single(e =>
                    GetPrimaryKeyValues(primaryKeys, e).SequenceEqual(primaryKayValues));

                bool isModified = IsModified(proxyEntity, entity);
                if (isModified)
                {
                    modifiedEntities.Add(entity);
                }
            }

            return modifiedEntities;
       }

       private bool IsModified(T proxyEntity, T entity)
       {
           PropertyInfo[] monitordProperties = typeof(T)
               .GetProperties()
               .Where(pi => DbContext.AllowedSqTypes.Contains(pi.PropertyType))
               .ToArray();

           PropertyInfo[] modifiedProperties = monitordProperties
               .Where(pi => !Equals(pi.GetValue(entity), pi.GetValue(proxyEntity)))
               .ToArray();

           return modifiedProperties.Any();
       }

       private IEnumerable<object> GetPrimaryKeyValues(IEnumerable<PropertyInfo> primaryKeys, T entity)
           => primaryKeys.Select(pi => pi.GetValue(entity)).ToArray();
       
           
       

       private IList<T> ClonedEntities(IEnumerable<T> entities)
       {
           IList<T> clonedEntities = new List<T>();

           PropertyInfo[] propertiesToClone =
               typeof(T)
                   .GetProperties()
                   .Where(pi => DbContext.AllowedSqTypes.Contains(pi.PropertyType))
                   .ToArray();

           foreach (var entity in entities)
           {
               var clonedEntity = Activator.CreateInstance<T>();

               foreach (var property in propertiesToClone)
               {
                   object value = property.GetValue(entity);
                   property.SetValue(clonedEntity, value);
               }

               clonedEntities.Add(clonedEntity);
           }

           return clonedEntities;
       }
    }
}