using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MiniORM.Messages;

namespace MiniORM
{
	// TODO: Create your DbSet class here.
    public class DbSet<TEntity> : ICollection<TEntity>
    where TEntity : class, new()
    {
        internal DbSet(IEnumerable<TEntity> entities)
        {
            Entities = entities.ToList();

            ChangeTracker = new ChangeTracker<TEntity>(Entities);
        }

        internal ChangeTracker<TEntity> ChangeTracker { get; set; }

        internal IList<TEntity> Entities { get; set; }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return this.Entities.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(TEntity item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item),ExceptionMessages.ItemNullException);
            }

            Entities.Add(item);
            ChangeTracker.Add(item);
        }

        public void Clear()
        {
            while (this.Entities.Any())
            {
                TEntity entity = this.Entities.First();
                this.Remove(entity);
            }
        }

        public bool Contains(TEntity item)
            => this.Entities.Contains(item);

        public void CopyTo(TEntity[] array, int arrayIndex)
        => this.Entities.CopyTo(array, arrayIndex);

        public bool Remove(TEntity item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item),ExceptionMessages.ItemNullException);
            }

            bool removedSuccessfully = this.Entities.Remove(item);

            if (removedSuccessfully)
            {
                this.ChangeTracker.Remove(item);
            }

            return removedSuccessfully;
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                this.Remove(entity);
            }
        }

        public int Count => this.Entities.Count;
        public bool IsReadOnly => this.Entities.IsReadOnly;
    }
}