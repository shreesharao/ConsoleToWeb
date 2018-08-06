using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleToWeb.Options
{
    public class LogOption
    {
        public string Default { get; set; }   //property name must be same as configuration key

        //Binding to an object graph. Create a property of a Class type 
        public DebugOption Debug { get; set; }
        public class DebugOption
        {
            public string Microsoft { get; set; }
            public string Default { get; set; }
        }
    }
}
