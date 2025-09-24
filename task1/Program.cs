using System;
using System.Collections.Generic;

class Point
{
    public int X { get; set; }
    public int Y { get; set; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
}

abstract class Shape
{
    public abstract bool Contains(Point p);
}

class Rectangle : Shape
{
    public Point topLeft { get; set; }
    public Point bottomRight { get; set; }

    public Rectangle(Point topLeft, Point bottomRight)
    {
        this.topLeft = topLeft;
        this.bottomRight = bottomRight;
    }

    public override bool Contains(Point p)
    {
        int minX = Math.Min(topLeft.X, bottomRight.X);
        int maxX = Math.Max(topLeft.X, bottomRight.X);
        int minY = Math.Min(topLeft.Y, bottomRight.Y);
        int maxY = Math.Max(topLeft.Y, bottomRight.Y);

        return (p.X >= minX && p.X <= maxX &&
                p.Y >= minY && p.Y <= maxY);
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Enter coordinates for rectangle:");

        int x1 = int.Parse(Console.ReadLine());
        int y1 = int.Parse(Console.ReadLine());
        int x2 = int.Parse(Console.ReadLine());
        int y2 = int.Parse(Console.ReadLine());

        Point topLeft = new Point(x1, y1);
        Point bottomRight = new Point(x2, y2);

        Shape rectangle = new Rectangle(topLeft, bottomRight);

        Console.WriteLine("Enter n for points:");
        int n = int.Parse(Console.ReadLine());

        List<Point> points = new List<Point>();
        for (int i = 0; i < n; i++)
        {
            Console.Write("Enter X: ");
            int px = int.Parse(Console.ReadLine());

            Console.Write("Enter Y: ");
            int py = int.Parse(Console.ReadLine());

            points.Add(new Point(px, py));
        }

        foreach (Point p in points)
        {
            Console.WriteLine("Result: " + rectangle.Contains(p));
        }
    }
}
