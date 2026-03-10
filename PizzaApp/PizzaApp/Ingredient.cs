using System;

namespace PizzaConsoleApp
{
    public class Ingredient : Entity
    {
        private string _name;
        private decimal _cost;

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Название ингредиента не может быть пустым.");
                _name = value;
            }
        }

        public decimal Cost
        {
            get => _cost;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Стоимость не может быть отрицательной.");
                _cost = value;
            }
        }

        public Ingredient(string name, decimal cost)
        {
            Name = name;
            Cost = cost;
        }

        public override string ToShortString() => $"[{Id}] {Name}";
        public override string ToString() => $"[{Id}] {Name} (стоимость: {Cost:F2})";
    }
}