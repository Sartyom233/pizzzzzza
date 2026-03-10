using System;

namespace PizzaConsoleApp
{
    public class DoughBase : Entity
    {
        private string _name;
        private decimal _cost;

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Название основы не может быть пустым.");
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

        public bool IsClassic { get; set; }

        public DoughBase(string name, decimal cost, bool isClassic)
        {
            Name = name;
            Cost = cost;
            IsClassic = isClassic;
        }

        public override string ToString() =>
            $"[{Id}] {Name} (стоимость: {Cost:F2}){(IsClassic ? " [классическая]" : "")}";
    }
}