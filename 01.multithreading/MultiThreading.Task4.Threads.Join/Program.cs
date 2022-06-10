/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        static Semaphore _semaphore;

        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();

            _semaphore = new Semaphore(2, 2);
            ThreadPool.SetMaxThreads(3, 3);
            CreateThread(10);
            Console.ReadLine();
        }

        static void CreateThread(object state)
        {
            _semaphore.WaitOne();
            if (!int.TryParse(state.ToString(), out int intState)) {
                throw new ArgumentException($"{nameof(state)} is not a number.");
            }

            if (intState == 0)
            {
                return;
            }

            var newState = --intState;
            Console.WriteLine($"Thread id: {Thread.CurrentThread.ManagedThreadId}, state: {newState}");
            Thread.Sleep(3000);

            //var thread = new Thread(CreateThread);
            
            //thread.Start(newState);
            //Thread.CurrentThread.Join();

            ThreadPool.QueueUserWorkItem(CreateThread, newState);
            _semaphore.Release();
        }
    }
}
