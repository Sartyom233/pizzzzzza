#nullable disable
using System;
using BeverageApp.Models;
using BeverageApp.Services;

namespace BeverageApp
{
    class Program
    {
        static BeverageCrud crud = new BeverageCrud();

        static void Main()
        {
            var latte = new Beverage("Латте");
            latte.AddStep(new GrindAction { Ingredient = new CoffeeBean() });
            latte.AddStep(new AddAction { Ingredient = new CoffeeBean(), Quantity = 18 });
            latte.AddStep(new BoilAction { Ingredient = new Water() });
            latte.AddStep(new PourAction { Ingredient = new Water() });          // упрощённый пролив
            latte.AddStep(new AddAction { Ingredient = new Milk(), Quantity = 200 });
            latte.AddStep(new WhiskAction { Ingredient = new Milk() });
            latte.AddStep(new MixAction { Ingredient = new Milk() });            // перемешивание
            crud.Create(latte);


            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n=== МЕНЮ ===");
                Console.WriteLine("1. Создать напиток");
                Console.WriteLine("2. Показать все напитки");
                Console.WriteLine("3. Показать рецепт");
                Console.WriteLine("4. Обновить напиток");
                Console.WriteLine("5. Удалить напиток");
                Console.WriteLine("6. Выход");
                Console.Write("Выбор: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": CreateBeverage(); break;
                    case "2": ListAll(); break;
                    case "3": PrintRecipe(); break;
                    case "4": UpdateBeverage(); break;
                    case "5": DeleteBeverage(); break;
                    case "6": exit = true; break;
                    default: Console.WriteLine("Неверный ввод"); break;
                }
            }
        }

        static void PrintElementInfo(IElement element)
        {
            Console.WriteLine($"Элемент: {element.GetType().Name}");
        }

        static void CreateBeverage()
        {
            Console.Write("Название напитка: ");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name)) return;

            var beverage = new Beverage(name);
            bool adding = true;
            while (adding)
            {
                Console.WriteLine("\nВыберите действие:");
                Console.WriteLine("1. Добавить ингредиент");
                Console.WriteLine("2. Вскипятить ингредиент");
                Console.WriteLine("3. Перемолоть ингредиент");
                Console.WriteLine("4. Пролить ингредиент");
                Console.WriteLine("5. Взбить ингредиент");
                Console.WriteLine("6. Перемешать ингредиент");
                Console.Write("Номер: ");
                string act = Console.ReadLine();

                BeverageAction action = null;

                Ingredient PickIngredient()
                {
                    Console.WriteLine("Ингредиент: 1-Вода, 2-Кофейное зерно, 3-Лёд, 4-Сироп, 5-Молоко");
                    string ing = Console.ReadLine();
                    switch (ing)
                    {
                        case "1": return new Water();
                        case "2": return new CoffeeBean();
                        case "3": return new Ice();
                        case "4": return new Syrup();
                        case "5": return new Milk();
                        default: return null;
                    }
                }

                switch (act)
                {
                    case "1":
                        var ing = PickIngredient();
                        if (ing == null) break;
                        Console.Write("Количество (г): ");
                        double qty = double.TryParse(Console.ReadLine(), out double q) ? q : 10;
                        action = new AddAction { Ingredient = ing, Quantity = qty };
                        break;
                    case "2":
                        var ing2 = PickIngredient();
                        if (ing2 == null) break;
                        action = new BoilAction { Ingredient = ing2 };
                        break;
                    case "3":
                        var ing3 = PickIngredient();
                        if (ing3 == null) break;
                        action = new GrindAction { Ingredient = ing3 };
                        break;
                    case "4":
                        var ing4 = PickIngredient();
                        if (ing4 == null) break;
                        action = new PourAction { Ingredient = ing4 };
                        break;
                    case "5":
                        var ing5 = PickIngredient();
                        if (ing5 == null) break;
                        action = new WhiskAction { Ingredient = ing5 };
                        break;
                    case "6":
                        var ing6 = PickIngredient();
                        if (ing6 == null) break;
                        action = new MixAction { Ingredient = ing6 };
                        break;
                    default:
                        Console.WriteLine("Неверное действие");
                        break;
                }

                if (action != null)
                {
                    beverage.AddStep(action);
                    Console.WriteLine("Действие добавлено.");
                }
                Console.Write("Добавить ещё? (y/n): ");
                adding = Console.ReadLine().ToLower() == "y";
            }
            crud.Create(beverage);
        }

        static void ListAll()
        {
            var all = crud.GetAll();
            if (all.Count == 0) Console.WriteLine("Нет напитков");
            else foreach (var b in all) Console.WriteLine($"- {b.Name} ({b.Steps.Count} шагов)");
        }

        static void PrintRecipe()
        {
            Console.Write("Название напитка: ");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name)) return;
            crud.PrintRecipe(name);
        }

        static void UpdateBeverage()
        {
            Console.Write("Название напитка для обновления: ");
            string oldName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(oldName)) return;
            var old = crud.Get(oldName);
            if (old == null) { Console.WriteLine("Не найден"); return; }

            Console.Write("Новое название (Enter - оставить): ");
            string newName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newName)) newName = old.Name;

            var updated = new Beverage(newName);
            Console.WriteLine("Введите новую последовательность действий:");

            bool adding = true;
            while (adding)
            {
                Console.WriteLine("\n1-Добавить,2-Вскипятить,3-Перемолоть,4-Пролить,5-Взбить,6-Перемешать");
                Console.Write("Номер: ");
                string act = Console.ReadLine();
                BeverageAction action = null;

                Ingredient PickIngredient()
                {
                    Console.WriteLine("Ингредиент: 1-Вода,2-Кофейное зерно,3-Лёд,4-Сироп,5-Молоко");
                    string ing = Console.ReadLine();
                    switch (ing)
                    {
                        case "1": return new Water();
                        case "2": return new CoffeeBean();
                        case "3": return new Ice();
                        case "4": return new Syrup();
                        case "5": return new Milk();
                        default: return null;
                    }
                }

                switch (act)
                {
                    case "1":
                        var ing = PickIngredient();
                        if (ing != null)
                        {
                            Console.Write("Количество (г): ");
                            double qty = double.TryParse(Console.ReadLine(), out double q) ? q : 10;
                            action = new AddAction { Ingredient = ing, Quantity = qty };
                        }
                        break;
                    case "2":
                        var ing2 = PickIngredient();
                        if (ing2 != null) action = new BoilAction { Ingredient = ing2 };
                        break;
                    case "3":
                        var ing3 = PickIngredient();
                        if (ing3 != null) action = new GrindAction { Ingredient = ing3 };
                        break;
                    case "4":
                        var ing4 = PickIngredient();
                        if (ing4 != null) action = new PourAction { Ingredient = ing4 };
                        break;
                    case "5":
                        var ing5 = PickIngredient();
                        if (ing5 != null) action = new WhiskAction { Ingredient = ing5 };
                        break;
                    case "6":
                        var ing6 = PickIngredient();
                        if (ing6 != null) action = new MixAction { Ingredient = ing6 };
                        break;
                }

                if (action != null)
                {
                    updated.AddStep(action);
                    Console.WriteLine("Действие добавлено.");
                }
                Console.Write("Добавить ещё? (y/n): ");
                adding = Console.ReadLine().ToLower() == "y";
            }
            crud.Update(oldName, updated);
        }

        static void DeleteBeverage()
        {
            Console.Write("Название напитка для удаления: ");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name)) return;
            crud.Delete(name);
        }
    }
}