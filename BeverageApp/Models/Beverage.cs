using System.Collections.Generic;

namespace BeverageApp.Models
{
    public class Beverage : IElement
    {
        public string Name { get; set; }
        public List<BeverageAction> Steps { get; set; }
        public Beverage(string name) { Name = name; Steps = new List<BeverageAction>(); }
        public void AddStep(BeverageAction action) => Steps.Add(action);
    }
}