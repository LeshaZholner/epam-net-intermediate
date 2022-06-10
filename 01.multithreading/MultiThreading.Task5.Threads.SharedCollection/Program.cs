/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        private static IList<int> _items;
        private static object _lock = new object();

        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            _items = new List<int>();

            try
            {
                var task = Task.Factory.StartNew(() => {
                    for (int i = 0; i < 10; i++)
                    {
                        object item = i;
                        AddItems(item);
                        var task2 = Task.Run(() => { PrintItems(); });
                        task2.Wait();
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();
        }

        static void AddItems(object item)
        {
            lock (_lock)
            {
                if (!int.TryParse(item.ToString(), out int intItem))
                {
                    throw new ArgumentException($"{nameof(item)} is not a number.");
                }

                _items.Add(intItem);
            }
        }

        static void PrintItems()
        {
            lock (_lock)
            {
                Console.WriteLine(string.Join(", ", _items.Select(x => x.ToString()).ToArray()));
            }
        }
    }
}
