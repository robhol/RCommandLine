using System.Reflection;
using RCommandLine;

namespace ExampleApplication
{
    class Program
    {
        static void Main()
        {
            
            //Step 1: define Options classes (see ExampleOptions.cs)
            //Step 2: instantiate Parser with desired Options type and settings
            var parser = Parser.FromAttributes<ExampleOptions>(
                new ParserOptions
                {
                    BaseCommandName =
                        System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location)
                }
                );
                

            //Step 3: Parse() and output consumption!
            var result = parser.Parse();
            if (result.Success) 
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
                        result.GetCommandList();
                        break;
                    }
                }
            }

        }

    }
}
