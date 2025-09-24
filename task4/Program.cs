using System;
using System.Collections.Generic;

abstract class Item
{
    public string Name;
    public long Amount;

    public Item(string name, long amount)
    {
        Name = name;
        Amount = amount;
    }

    public abstract string GetTypeName();
}

class Gold : Item
{
    public Gold(string name, long amount) : base(name, amount) { }

    public override string GetTypeName()
    {
        return "Gold";
    }
}

class Gem : Item
{
    public Gem(string name, long amount) : base(name, amount) { }

    public override string GetTypeName()
    {
        return "Gem";
    }
}

class Cash : Item
{
    public Cash(string name, long amount) : base(name, amount) { }

    public override string GetTypeName()
    {
        return "Cash";
    }
}
class Bag
{
    public long Capacity;
    private List<Item> items;

    public Bag(long capacity)
    {
        Capacity = capacity;
        items = new List<Item>();
    }

    public long GetTotal()
    {
        long total = 0;
        for (int i = 0; i < items.Count; i++)
        {
            total += items[i].Amount;
        }
        return total;
    }

    public long GetTotalByType(string type)
    {
        long total = 0;
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].GetTypeName() == type)
                total += items[i].Amount;
        }
        return total;
    }

    // Додаємо предмет у мішок
    public bool TryAdd(Item item)
    {
        long totalGold = GetTotalByType("Gold");
        long totalGem = GetTotalByType("Gem");
        long totalCash = GetTotalByType("Cash");

        if (GetTotal() + item.Amount > Capacity)
            return false; // перевищує місткість

        if (item is Gold)
        {
            items.Add(item);
            return true;
        }
        else if (item is Gem)
        {
            if (totalGold >= totalGem + item.Amount)
            {
                items.Add(item);
                return true;
            }
        }
        else if (item is Cash)
        {
            if (totalGem >= totalCash + item.Amount)
            {
                items.Add(item);
                return true;
            }
        }

        return false; // не відповідає правилам
    }

    // Вивід мішка
    public void Print()
    {
        string[] types = { "Gold", "Gem", "Cash" };

        // Сортуємо типи за сумою (спадання)
        for (int t = 0; t < types.Length; t++)
        {
            long sum = GetTotalByType(types[t]);
            if (sum == 0)
                continue;

            Console.WriteLine("<" + types[t] + "> $" + sum);

            // Всі предмети цього типу
            List<Item> itemsOfType = new List<Item>();
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].GetTypeName() == types[t])
                    itemsOfType.Add(items[i]);
            }

            // Сортуємо за назвою спадання, потім за сумою зростання
            for (int i = 0; i < itemsOfType.Count - 1; i++)
            {
                for (int j = i + 1; j < itemsOfType.Count; j++)
                {
                    if (string.Compare(itemsOfType[i].Name, itemsOfType[j].Name) < 0 ||
                        (itemsOfType[i].Name == itemsOfType[j].Name && itemsOfType[i].Amount > itemsOfType[j].Amount))
                    {
                        // міняємо місцями
                        Item temp = itemsOfType[i];
                        itemsOfType[i] = itemsOfType[j];
                        itemsOfType[j] = temp;
                    }
                }
            }

            // Вивід предметів
            for (int i = 0; i < itemsOfType.Count; i++)
            {
                Console.WriteLine("##" + itemsOfType[i].Name + " - " + itemsOfType[i].Amount);
            }
        }
    }
}

public class Program
{
    static void Main()
    {
        long capacity = long.Parse(Console.ReadLine());
        string[] input = Console.ReadLine().Split(' ');

        Bag bag = new Bag(capacity);

        for (int i = 0; i < input.Length; i += 2)
        {
            string name = input[i];
            long amount = long.Parse(input[i + 1]);

            Item item = null;

            // Визначаємо тип предмета
            if (name.Equals("Gold", StringComparison.OrdinalIgnoreCase))
                item = new Gold(name, amount);
            else if (name.Length == 3)
                item = new Cash(name, amount);
            else if (name.Length >= 4 && name.EndsWith("Gem", StringComparison.OrdinalIgnoreCase))
                item = new Gem(name, amount);

            if (item != null)
            {
                bag.TryAdd(item);
            }
        }

        bag.Print();
    }
}


