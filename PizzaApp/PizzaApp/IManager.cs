using System.Collections.Generic;

namespace PizzaConsoleApp
{
    public interface IManager<T> where T : Entity
    {
        T Create(T entity);
        T Update(int id, T newEntity);
        bool Delete(int id);
        T GetById(int id);
        IEnumerable<T> GetAll();
    }
}