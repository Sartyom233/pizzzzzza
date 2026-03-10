using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PizzaConsoleApp
{
    public class Pizza : Entity
    {
        private string _name;
        private List<Ingredient> _ingredients;
        private DoughBase _doughBase;

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new System.ArgumentException("Название пиццы не может быть пустым.");
                _name = value;
            }
        }

        public IReadOnlyList<Ingredient> Ingredients => _ingredients.AsReadOnly();
        public DoughBase DoughBase
        {
            get => _doughBase;
            set
            {
                if (value == null)
                    throw new System.ArgumentNullException(nameof(value), "Основа не может быть null.");
                _doughBase = value;
            }
        }

        public Pizza(string name, DoughBase doughBase)
        {
            Name = name;
            DoughBase = doughBase ?? throw new System.ArgumentNullException(nameof(doughBase));
            _ingredients = new List<Ingredient>();
        }

        public void AddIngredient(Ingredient ingredient)
        {
            if (ingredient == null)
                throw new System.ArgumentNullException(nameof(ingredient));
            _ingredients.Add(ingredient);
        }

        public bool RemoveIngredient(int ingredientId)
        {
            var ingredient = _ingredients.FirstOrDefault(i => i.Id == ingredientId);
            if (ingredient != null)
                return _ingredients.Remove(ingredient);
            return false;
        }

        public decimal CalculateTotalCost()
        {
            decimal ingredientsCost = _ingredients.Sum(i => i.Cost);
            return ingredientsCost + DoughBase.Cost;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"[{Id}] {Name} (основа: {DoughBase.Name})");
            sb.AppendLine("Ингредиенты:");
            foreach (var ing in _ingredients)
                sb.AppendLine($"  {ing.ToShortString()}");
            sb.AppendLine($"Общая стоимость: {CalculateTotalCost():F2}");
            return sb.ToString();
        }
    }
}