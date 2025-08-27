namespace MyFirstProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            //array = a variable that can staor multiple values . fixed size

            String[] cars = { "BMW", "Mustang", "Corvette" };

            cars[0] = "Tesla";


            for (int i = 0; i < cars.Length; i++)
            {
                Console.WriteLine(cars[i]);
                Console.WriteLine(cars[1]);


            }

            Console.ReadKey();

        }
    }
}