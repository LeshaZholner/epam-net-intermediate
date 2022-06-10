/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            //var task1 = Task.Run(() =>
            //{
            //    Console.WriteLine("Task 1: Parent task");
            //    throw new Exception("Some Exception");
            //}).ContinueWith(t =>
            //{
            //    Console.WriteLine("Task 1: Cantinuation task");
            //});

            //var task2 = Task.Run(() =>
            //{
            //    Console.WriteLine("Task 2: Parent task");
            //    throw new Exception("Some Exception");
            //}).ContinueWith(t =>
            //{
            //    Console.WriteLine("Task 2: Cantinuation task");
            //}, TaskContinuationOptions.OnlyOnFaulted);

            //var task3 = Task.Run(() =>
            //{
            //    Console.WriteLine("Task 3: Parent task");
            //    Console.WriteLine($"Thread id: {Thread.CurrentThread.ManagedThreadId}");
            //    throw new Exception("Some Exception");
            //}).ContinueWith(t =>
            //{
            //    Console.WriteLine("Task 3: Cantinuation task");
            //    Console.WriteLine($"Thread id: {Thread.CurrentThread.ManagedThreadId}");
            //}, TaskContinuationOptions.ExecuteSynchronously);

            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;

            var task4 = Task.Run(() =>
            {
                Console.WriteLine("Task 4: Parent task");
                Console.WriteLine($"Thread id: {Thread.CurrentThread.ManagedThreadId}");
            }, token).ContinueWith(t =>
            {
                Console.WriteLine("Task 4: Cantinuation task");
                Console.WriteLine($"Task 4 parent status: {t.Status}");
                Console.WriteLine($"Thread id: {Thread.CurrentThread.ManagedThreadId}");
            }, TaskContinuationOptions.OnlyOnCanceled);
            cancelTokenSource.Cancel();
            
            
            Console.ReadLine();
        }
    }
}
