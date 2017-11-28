using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleToWeb.Services.Abstractions
{
    public interface INotifier
    {
        void Notify(string message);
    }
}
