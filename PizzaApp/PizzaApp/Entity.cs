using System;

namespace PizzaConsoleApp
{
    public abstract class Entity
    {
        private static int _nextId = 1;
        public int Id { get; }

        protected Entity()
        {
            Id = _nextId++;
        }

        public virtual string ToShortString() => $"ID: {Id}";
    }
}