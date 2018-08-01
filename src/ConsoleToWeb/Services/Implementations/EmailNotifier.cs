using ConsoleToWeb.Services.Abstractions;
using System;

namespace ConsoleToWeb.Services.Implementations
{
    public class EmailNotifier : INotifier
    {
        public void Notify(string message)
        {
            Console.WriteLine("EmailNotifier Notify method called..");
        }
    }
}
