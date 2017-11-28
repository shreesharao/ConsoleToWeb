using ConsoleToWeb.Services.Abstractions;
using System;

namespace ConsoleToWeb.Services.Implementations
{
    public class ConsoleNotifier : INotifier
    {
        public ConsoleNotifier()
        {
            Console.WriteLine("ConsoleNotifier constructor reached");
        }
        public void Notify(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
