using System;

namespace TriangleTypeIdentifier
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the lengths of the three sides of the triangle:");
            Console.Write("Side A: ");
            double sideA = Convert.ToDouble(Console.ReadLine());
            Console.Write("Side B: ");
            double sideB = Convert.ToDouble(Console.ReadLine());
            Console.Write("Side C: ");
            double sideC = Convert.ToDouble(Console.ReadLine());
            string triangleType = IdentifyTriangleType(sideA, sideB, sideC);
            Console.WriteLine($"The triangle is: {triangleType}");

            Console.Beep();
        }
        static string IdentifyTriangleType(double a, double b, double c)
        {
            if (a <= 0 || b <= 0 || c <= 0)
                return "Invalid triangle sides";
            if (a == b && b == c)
                return "Equilateral";
            else if (a == b || b == c || a == c)
                return "Isosceles";
            else
                return "Scalene";
        }

      

    }
}
