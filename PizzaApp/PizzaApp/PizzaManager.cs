using System;
using System.Collections.Generic;
using System.Linq;

namespace PizzaConsoleApp
{
    public class PizzaManager : Manager<Pizza>
    {
        private readonly Manager<DoughBase> _doughManager;
        private readonly Manager<Ingredient> _ingredientManager;

        public PizzaManager(Manager<DoughBase> doughManager, Manager<Ingredient> ingredientManager)
        {
            _doughManager = doughManager;
            _ingredientManager = ingredientManager;
        }

        public override Pizza Create(Pizza pizza)
        {
            if (pizza.DoughBase == null)
                throw new InvalidOperationException("Пицца должна иметь основу.");
            return base.Create(pizza);
        }

        public bool AddIngredientToPizza(int pizzaId, int ingredientId)
        {
            var pizza = GetById(pizzaId);
            var ingredient = _ingredientManager.GetById(ingredientId);
            if (pizza == null || ingredient == null)
                return false;
            pizza.AddIngredient(ingredient);
            return true;
        }

        public bool RemoveIngredientFromPizza(int pizzaId, int ingredientId)
        {
            var pizza = GetById(pizzaId);
            return pizza?.RemoveIngredient(ingredientId) ?? false;
        }
    }
}