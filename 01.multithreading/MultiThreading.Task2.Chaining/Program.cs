/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        const int MaxValue = 100;

        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();
                        
            var result = Task.Run(() =>
            {
                var random = new Random();
                int[] numbers = new int[10];
                for (int i = 0; i < numbers.Length; i++)
                {
                    numbers[i] = random.Next(MaxValue);
                }
                PrintArray(numbers);
                return numbers;
            })
            .ContinueWith(x =>
            {
                var randomNumber = new Random().Next(MaxValue);
                var numbers = x.Result;
                for (int i = 0; i < numbers.Length; i++)
                {
                    numbers[i] *= randomNumber;
                }
                PrintArray(numbers);
                return numbers;
            })
            .ContinueWith(x =>
            {
                var orderedNumbers = x.Result.OrderBy(num => num).ToArray();
                PrintArray(orderedNumbers);
                return orderedNumbers;
            })
            .ContinueWith(x =>
            {
                var numbers = x.Result;
                var avgValue = (double)numbers.Sum() / numbers.Length;
                Console.WriteLine(avgValue);
                return avgValue;
            });
            result.Wait();

            Console.ReadLine();
        }

        static string ArrayToString(int[] array)
        {
            return string.Join(", ", array.Select(x => x.ToString()).ToArray());
        }

        static void PrintArray(int[] array)
        {
            Console.WriteLine(ArrayToString(array));
        }
    }
}
