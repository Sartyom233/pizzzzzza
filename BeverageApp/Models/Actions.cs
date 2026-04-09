#nullable disable
using System.Collections.Generic;

namespace BeverageApp.Models
{
    public abstract class BeverageAction : IElement
    {
        public List<IElement> Elements { get; set; } = new List<IElement>();
        private Ingredient _ingredient;
        public Ingredient Ingredient
        {
            get => _ingredient;
            set => _ingredient = value;
        }

        public abstract string Description { get; }
    }

    public class AddAction : BeverageAction
    {
        private double _quantity;
        public double Quantity
        {
            get => _quantity;
            set => _quantity = value;
        }

        public override string Description => $"Добавить {Quantity} г {Ingredient?.RussianName ?? "ингредиента"}";
    }

    public class BoilAction : BeverageAction
    {
        public override string Description => $"Вскипятить {Ingredient?.RussianName ?? "ингредиент"}";
    }

    public class GrindAction : BeverageAction
    {
        public override string Description => $"Перемолоть {Ingredient?.RussianName ?? "ингредиент"}";
    }

    public class PourAction : BeverageAction
    {
        public override string Description => $"Пролить {Ingredient?.RussianName ?? "ингредиент"}";
    }

    public class WhiskAction : BeverageAction
    {
        public override string Description => $"Взбить {Ingredient?.RussianName ?? "ингредиент"}";
    }

    public class MixAction : BeverageAction
    {
        public override string Description => $"Перемешать {Ingredient?.RussianName ?? "ингредиент"}";
    }
}