using System;

namespace TicketPriceCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            int Ticket_Price = 10;
            Console.WriteLine("Welcome to the Ticket Price Calculator!");
            Console.Write("Enter your age: ");
            int age = int.Parse(Console.ReadLine());

            if (age <= 12 || age >= 65)
            {
                Ticket_Price -= 7;
            }

            Console.WriteLine($"Your ticket price is: {Ticket_Price}");
        }
    }
}