using System;


namespace GradeCalculator

{
    class Program
    {
        static void Main(string[] args)
        {
        double grade ;

        Console.WriteLine("Input your grade 1 - 100");
        grade = Convert.ToDouble( Console.ReadLine());


        Console.WriteLine(grade);

        if (grade >= 90)
            {
                Console.WriteLine("You got an A");
            }
        else if (grade >= 80)
            {
                Console.WriteLine("You got a B");
            }
        else if (grade >= 70)
            {
                Console.WriteLine("You got a C");
            }
        else if (grade >= 60)
            {
                Console.WriteLine("You got a D");
            }
        else if (grade >= 0)
            {
                Console.WriteLine("You got an F");
            }


        
                Console.Beep();
        }
    }

}

