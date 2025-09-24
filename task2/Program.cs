using System;

enum Season
{
    Spring = 2,
    Summer = 4,
    Autumn = 1,
    Winter = 3
}

enum TypeDiscount
{
    VIP,
    None,
    SecondVisit
}

abstract class PriceCalculatorBase
{
    public double pricePerDay { get; set; }
    public double numberOfDays { get; set; }
    public Season season { get; set; }
    public TypeDiscount discount { get; set; }

    public PriceCalculatorBase(double pricePerDay, double numberOfDays, Season season, TypeDiscount discount)
    {
        this.pricePerDay = pricePerDay; 
        this.numberOfDays = numberOfDays;
        this.season = season;
        this.discount = discount;
    }

    public abstract double CalculatePrice();
}

class PriceCalculation : PriceCalculatorBase
{
    public PriceCalculation(double pricePerDay, double numberOfDays, Season season, TypeDiscount discount)
        : base(pricePerDay, numberOfDays, season, discount) { }

    public override double CalculatePrice()
    {
        double basePrice = pricePerDay * numberOfDays * (int)season;
        double applyDiscount = 1.0;

        if (discount == TypeDiscount.VIP)
        {
            applyDiscount = 0.8;
        }
        else if (discount == TypeDiscount.SecondVisit)
        {
            applyDiscount = 0.9;
        }

        return basePrice * applyDiscount;
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Enter price per day, number of days, season and discount type (optional):");
        string[] input = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        double pricePerDay = double.Parse(input[0]);
        int numberOfDays = int.Parse(input[1]);
        Season season = Enum.Parse<Season>(input[2], true);

        TypeDiscount discount = TypeDiscount.None;
        if (input.Length == 4) discount = Enum.Parse<TypeDiscount>(input[3], true);

        PriceCalculatorBase calculator = new PriceCalculation(pricePerDay, numberOfDays, season, discount);
        double totalPrice = calculator.CalculatePrice();

        Console.WriteLine($"{totalPrice:F2}");
    }
}
