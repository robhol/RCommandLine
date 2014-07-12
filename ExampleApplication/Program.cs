using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RCommandLine;

namespace ExampleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            
            //Step 1: define Options classes (see ExampleOptions.cs)
            //Step 2: instantiate Parser with desired Options type and settings
            var parser = new Parser<ExampleOptions>
            {
                BaseCommandName = System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location),
                AutomaticCommandList = true,
                AutomaticUsage = true,
                AutomaticHelp = true
            };

            //Step 3: Parse() and output consumption!
            var result = parser.Parse();
            if (result != ParseResult.None) 
            {
                switch (result.Command)
                {
                    case "read":
                    {
                        var x = result.Options as ExampleOptions.ReadOptions;
                        // your logic here
                        break;
                    }
                    case "write":
                    {
                        var x = result.Options as ExampleOptions.WriteOptions;

                        break;
                    }
                    default:
                    {
                        //by design, AutomaticCommandList doesn't catch "invalid" commands, just empty ones.
                        result.ShowCommandList();
                        break;
                    }
                }
            }

        }

    }
}
