namespace BeverageApp.Models
{
    public abstract class Ingredient : IElement
    {
        private double _netMass;
        public double NetMass { get => _netMass; set => _netMass = value; }
        public virtual string RussianName => "Ingredient";
        protected Ingredient(double netMass) { _netMass = netMass; }
    }

    public class Water : Ingredient { public Water() : base(1000) { } public override string RussianName => "Вода"; }
    public class CoffeeBean : Ingredient { public CoffeeBean() : base(500) { } public override string RussianName => "Кофейное зерно"; }
    public class Ice : Ingredient { public Ice() : base(200) { } public override string RussianName => "Лёд"; }
    public class Syrup : Ingredient { public Syrup() : base(250) { } public override string RussianName => "Сироп"; }
    public class Milk : Ingredient { public Milk() : base(1000) { } public override string RussianName => "Молоко"; }
}