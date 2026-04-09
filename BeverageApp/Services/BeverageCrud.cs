#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using BeverageApp.Models;

namespace BeverageApp.Services
{
    public class BeverageCrud
    {
        private List<Beverage> beverages = new List<Beverage>();
        public void Create(Beverage beverage) { beverages.Add(beverage); Console.WriteLine($"Напиток \"{beverage.Name}\" добавлен."); }
        public List<Beverage> GetAll() => beverages;
        public Beverage Get(string name) => beverages.FirstOrDefault(b => b.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        public void Update(string oldName, Beverage newBeverage)
        {
            var existing = Get(oldName);
            if (existing == null) { Console.WriteLine("Напиток не найден"); return; }
            existing.Name = newBeverage.Name;
            existing.Steps = newBeverage.Steps;
            Console.WriteLine($"Напиток \"{oldName}\" обновлён.");
        }
        public void Delete(string name)
        {
            var beverage = Get(name);
            if (beverage != null) { beverages.Remove(beverage); Console.WriteLine($"Напиток \"{name}\" удалён."); }
            else Console.WriteLine("Напиток не найден");
        }
        public void PrintRecipe(string name)
        {
            var beverage = Get(name);
            if (beverage == null) { Console.WriteLine("Напиток не найден"); return; }
            Console.WriteLine($"\n=== {beverage.Name} ===");
            if (beverage.Steps.Count == 0) Console.WriteLine("(последовательность действий пуста)");
            else for (int i = 0; i < beverage.Steps.Count; i++) Console.WriteLine($"{i + 1}. {beverage.Steps[i].Description}");
            Console.WriteLine();
        }
    }
}