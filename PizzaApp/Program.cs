using System;
using System.Collections.Generic;
using System.Linq;

namespace PizzaConsoleApp
{
    class Program
    {
        private static readonly Manager<Ingredient> ingredientManager = new Manager<Ingredient>();
        private static readonly Manager<DoughBase> doughManager = new Manager<DoughBase>();
        private static readonly PizzaManager pizzaManager = new PizzaManager(doughManager, ingredientManager);
        private static DoughBase classicDough;

        static void Main(string[] args)
        {
            classicDough = new DoughBase("Классическое", 100, true);
            doughManager.Create(classicDough);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Система управления пиццерией");
                Console.WriteLine("1. Управление ингредиентами");
                Console.WriteLine("2. Управление типами основы");
                Console.WriteLine("3. Управление пиццами");
                Console.WriteLine("4. Выход");
                Console.Write("Выберите пункт: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ManageIngredients(); break;
                    case "2": ManageDoughs(); break;
                    case "3": ManagePizzas(); break;
                    case "4": return;
                    default: Console.WriteLine("Неверный ввод. Нажмите любую клавишу..."); Console.ReadKey(); break;
                }
            }
        }

        static void ManageIngredients()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("--- Управление ингредиентами ---");
                Console.WriteLine("1. Список ингредиентов");
                Console.WriteLine("2. Добавить ингредиент");
                Console.WriteLine("3. Редактировать ингредиент");
                Console.WriteLine("4. Удалить ингредиент");
                Console.WriteLine("5. Назад");
                Console.Write("Выберите: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ListIngredients(); break;
                    case "2": AddIngredient(); break;
                    case "3": EditIngredient(); break;
                    case "4": DeleteIngredient(); break;
                    case "5": return;
                    default: Console.WriteLine("Неверный ввод."); break;
                }
                Console.WriteLine("Нажмите любую клавишу...");
                Console.ReadKey();
            }
        }

        static void ListIngredients()
        {
            var ingredients = ingredientManager.GetAll();
            if (!ingredients.Any())
                Console.WriteLine("Список пуст.");
            else
                foreach (var i in ingredients)
                    Console.WriteLine(i);
        }

        static void AddIngredient()
        {
            try
            {
                Console.Write("Название: ");
                string name = Console.ReadLine();
                Console.Write("Стоимость: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal cost))
                {
                    var ing = new Ingredient(name, cost);
                    ingredientManager.Create(ing);
                    Console.WriteLine("Ингредиент добавлен.");
                }
                else Console.WriteLine("Неверная стоимость.");
            }
            catch (Exception ex) { Console.WriteLine($"Ошибка: {ex.Message}"); }
        }

        static void EditIngredient()
        {
            ListIngredients();
            Console.Write("Введите ID ингредиента для редактирования: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var existing = ingredientManager.GetById(id);
                if (existing == null) { Console.WriteLine("Не найден."); return; }
                try
                {
                    Console.Write($"Новое название (было {existing.Name}): ");
                    string name = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(name)) name = existing.Name;
                    Console.Write($"Новая стоимость (была {existing.Cost}): ");
                    string costStr = Console.ReadLine();
                    decimal cost = string.IsNullOrWhiteSpace(costStr) ? existing.Cost : decimal.Parse(costStr);
                    var updated = new Ingredient(name, cost);
                    ingredientManager.Update(id, updated);
                    Console.WriteLine("Ингредиент обновлён.");
                }
                catch (Exception ex) { Console.WriteLine($"Ошибка: {ex.Message}"); }
            }
        }

        static void DeleteIngredient()
        {
            ListIngredients();
            Console.Write("Введите ID ингредиента для удаления: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                if (ingredientManager.Delete(id))
                    Console.WriteLine("Удалён.");
                else Console.WriteLine("Не найден.");
            }
        }

        static void ManageDoughs()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("--- Управление типами основы ---");
                Console.WriteLine("1. Список основ");
                Console.WriteLine("2. Добавить основу");
                Console.WriteLine("3. Редактировать основу");
                Console.WriteLine("4. Удалить основу");
                Console.WriteLine("5. Назад");
                Console.Write("Выберите: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ListDoughs(); break;
                    case "2": AddDough(); break;
                    case "3": EditDough(); break;
                    case "4": DeleteDough(); break;
                    case "5": return;
                    default: Console.WriteLine("Неверный ввод."); break;
                }
                Console.WriteLine("Нажмите любую клавишу...");
                Console.ReadKey();
            }
        }

        static void ListDoughs()
        {
            var doughs = doughManager.GetAll();
            if (!doughs.Any())
                Console.WriteLine("Список пуст.");
            else
                foreach (var d in doughs)
                    Console.WriteLine(d);
        }

        static void AddDough()
        {
            try
            {
                Console.Write("Название: ");
                string name = Console.ReadLine();
                Console.Write("Стоимость: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal cost))
                {
                    Console.Write("Это классическая основа? (y/n): ");
                    bool isClassic = Console.ReadLine().Trim().ToLower() == "y";
                    if (!isClassic && classicDough != null)
                    {
                        decimal maxCost = classicDough.Cost * 1.2m;
                        if (cost > maxCost)
                        {
                            Console.WriteLine($"Ошибка: стоимость не должна превышать 120% классической ({maxCost:F2})");
                            return;
                        }
                    }
                    var dough = new DoughBase(name, cost, isClassic);
                    doughManager.Create(dough);
                    Console.WriteLine("Основа добавлена.");
                }
                else Console.WriteLine("Неверная стоимость.");
            }
            catch (Exception ex) { Console.WriteLine($"Ошибка: {ex.Message}"); }
        }

        static void EditDough()
        {
            ListDoughs();
            Console.Write("Введите ID основы для редактирования: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var existing = doughManager.GetById(id);
                if (existing == null) { Console.WriteLine("Не найден."); return; }
                try
                {
                    Console.Write($"Новое название (было {existing.Name}): ");
                    string name = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(name)) name = existing.Name;
                    Console.Write($"Новая стоимость (была {existing.Cost}): ");
                    string costStr = Console.ReadLine();
                    decimal cost = string.IsNullOrWhiteSpace(costStr) ? existing.Cost : decimal.Parse(costStr);
                    Console.Write($"Классическая? (было {(existing.IsClassic ? "да" : "нет")}) (y/n, оставьте пустым для без изменений): ");
                    string classicInput = Console.ReadLine();
                    bool isClassic = existing.IsClassic;
                    if (!string.IsNullOrWhiteSpace(classicInput))
                        isClassic = classicInput.Trim().ToLower() == "y";

                    if (!isClassic && classicDough != null && existing.Id != classicDough.Id)
                    {
                        decimal maxCost = classicDough.Cost * 1.2m;
                        if (cost > maxCost)
                        {
                            Console.WriteLine($"Ошибка: стоимость не должна превышать 120% классической ({maxCost:F2})");
                            return;
                        }
                    }
                    var updated = new DoughBase(name, cost, isClassic);
                    doughManager.Update(id, updated);
                    Console.WriteLine("Основа обновлена.");
                }
                catch (Exception ex) { Console.WriteLine($"Ошибка: {ex.Message}"); }
            }
        }

        static void DeleteDough()
        {
            ListDoughs();
            Console.Write("Введите ID основы для удаления: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var dough = doughManager.GetById(id);
                if (dough != null && dough.IsClassic)
                {
                    Console.WriteLine("Нельзя удалить классическую основу.");
                    return;
                }
                if (doughManager.Delete(id))
                    Console.WriteLine("Удалена.");
                else Console.WriteLine("Не найдена.");
            }
        }

        static void ManagePizzas()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("--- Управление пиццами ---");
                Console.WriteLine("1. Список пицц");
                Console.WriteLine("2. Добавить пиццу");
                Console.WriteLine("3. Редактировать пиццу");
                Console.WriteLine("4. Удалить пиццу");
                Console.WriteLine("5. Добавить ингредиент в пиццу");
                Console.WriteLine("6. Удалить ингредиент из пиццы");
                Console.WriteLine("7. Назад");
                Console.Write("Выберите: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ListPizzas(); break;
                    case "2": AddPizza(); break;
                    case "3": EditPizza(); break;
                    case "4": DeletePizza(); break;
                    case "5": AddIngredientToPizza(); break;
                    case "6": RemoveIngredientFromPizza(); break;
                    case "7": return;
                    default: Console.WriteLine("Неверный ввод."); break;
                }
                Console.WriteLine("Нажмите любую клавишу...");
                Console.ReadKey();
            }
        }

        static void ListPizzas()
        {
            var pizzas = pizzaManager.GetAll();
            if (!pizzas.Any())
                Console.WriteLine("Список пуст.");
            else
                foreach (var p in pizzas)
                    Console.WriteLine(p);
        }

        static void AddPizza()
        {
            try
            {
                Console.Write("Название пиццы: ");
                string name = Console.ReadLine();
                ListDoughs();
                Console.Write("Введите ID основы: ");
                if (!int.TryParse(Console.ReadLine(), out int doughId)) return;
                var dough = doughManager.GetById(doughId);
                if (dough == null) { Console.WriteLine("Основа не найдена."); return; }
                var pizza = new Pizza(name, dough);
                pizzaManager.Create(pizza);
                Console.WriteLine("Пицца создана.");
            }
            catch (Exception ex) { Console.WriteLine($"Ошибка: {ex.Message}"); }
        }

        static void EditPizza()
        {
            ListPizzas();
            Console.Write("Введите ID пиццы для редактирования: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var existing = pizzaManager.GetById(id);
                if (existing == null) { Console.WriteLine("Не найдена."); return; }
                try
                {
                    Console.Write($"Новое название (было {existing.Name}): ");
                    string name = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(name)) name = existing.Name;
                    ListDoughs();
                    Console.Write($"Новый ID основы (текущий {existing.DoughBase.Id}): ");
                    string doughInput = Console.ReadLine();
                    DoughBase dough = existing.DoughBase;
                    if (!string.IsNullOrWhiteSpace(doughInput) && int.TryParse(doughInput, out int doughId))
                    {
                        var newDough = doughManager.GetById(doughId);
                        if (newDough == null) { Console.WriteLine("Основа не найдена."); return; }
                        dough = newDough;
                    }
                    var updated = new Pizza(name, dough);
                    foreach (var ing in existing.Ingredients)
                        updated.AddIngredient(ing);
                    pizzaManager.Update(id, updated);
                    Console.WriteLine("Пицца обновлена.");
                }
                catch (Exception ex) { Console.WriteLine($"Ошибка: {ex.Message}"); }
            }
        }

        static void DeletePizza()
        {
            ListPizzas();
            Console.Write("Введите ID пиццы для удаления: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                if (pizzaManager.Delete(id))
                    Console.WriteLine("Удалена.");
                else Console.WriteLine("Не найдена.");
            }
        }

        static void AddIngredientToPizza()
        {
            ListPizzas();
            Console.Write("Введите ID пиццы: ");
            if (!int.TryParse(Console.ReadLine(), out int pizzaId)) return;
            var pizza = pizzaManager.GetById(pizzaId);
            if (pizza == null) { Console.WriteLine("Пицца не найдена."); return; }
            ListIngredients();
            Console.Write("Введите ID ингредиента: ");
            if (!int.TryParse(Console.ReadLine(), out int ingId)) return;
            if (pizzaManager.AddIngredientToPizza(pizzaId, ingId))
                Console.WriteLine("Ингредиент добавлен.");
            else Console.WriteLine("Ошибка добавления.");
        }

        static void RemoveIngredientFromPizza()
        {
            ListPizzas();
            Console.Write("Введите ID пиццы: ");
            if (!int.TryParse(Console.ReadLine(), out int pizzaId)) return;
            var pizza = pizzaManager.GetById(pizzaId);
            if (pizza == null) { Console.WriteLine("Пицца не найдена."); return; }
            foreach (var ing in pizza.Ingredients)
                Console.WriteLine(ing.ToShortString());
            Console.Write("Введите ID ингредиента для удаления: ");
            if (!int.TryParse(Console.ReadLine(), out int ingId)) return;
            if (pizzaManager.RemoveIngredientFromPizza(pizzaId, ingId))
                Console.WriteLine("Удалён.");
            else Console.WriteLine("Ошибка удаления.");
        }
    }
}