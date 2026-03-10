using System;
using System.Collections.Generic;
using System.Linq;

namespace PizzaConsoleApp
{
    public class Manager<T> : IManager<T> where T : Entity
    {
        protected readonly List<T> _items = new List<T>();

        public virtual T Create(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            _items.Add(entity);
            return entity;
        }

        public virtual T Update(int id, T newEntity)
        {
            if (newEntity == null)
                throw new ArgumentNullException(nameof(newEntity));
            var existing = GetById(id);
            if (existing == null)
                return null;
            _items.Remove(existing);
            _items.Add(newEntity);
            return newEntity;
        }

        public virtual bool Delete(int id)
        {
            var entity = GetById(id);
            if (entity == null)
                return false;
            return _items.Remove(entity);
        }

        public T GetById(int id) => _items.FirstOrDefault(e => e.Id == id);
        public IEnumerable<T> GetAll() => _items.ToList();
    }
}