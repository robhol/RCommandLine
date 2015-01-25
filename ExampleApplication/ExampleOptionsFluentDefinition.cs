using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleApplication
{
    using RCommandLine.Parsers;

    class ExampleOptionsFluentDefinition
    {

        public ExampleOptionsFluentDefinition()
        {

            var parser = Parser.CreateFluently<ExampleOptions>(builder => builder
                .BaseCommand<ExampleOptions.FileOptions>(fo => fo
                    .Argument(x => x.Path, c => c
                        .Description("The file to act upon"))
                    .Flag(x => x.Format, c => c
                        .ShortName('F')
                        .Optional("foo")
                        .Description("The format to use in a file operation"))
                )
                .Command<ExampleOptions.ReadOptions>(ro => ro
                    .Description("Read a file")

                    .Usage(@"""input file.txt""", "Reads a file using the default settings for format and chunk size.")
                    .Usage(@"""input file.txt"" -cF 4096 ASCII", "Reads a file using format ASCII and chunk size 4096.")

                    .Remark("For more information regarding chunk sizes, consult the manual.")

                    .Flag(x => x.ChunkSize, c => c
                        .ShortName('c')
                        .Optional(2048))
                )
                .Command<ExampleOptions.WriteOptions>(wo => wo
                    .Usage(@"""output file.txt""", "Writes a file with standard settings.")
                    .Usage(@"""output file.txt"" -dF XML", "Writes a file in XML format, including debug information.")

                    .Remark("If the file exists, it will be overwritten without warning.")

                    .Flag(x => x.DebugInfo, c => c
                        .ShortName('d')
                        .Description("Whether or not to include debug information in the output"))
                ));

        }


    }
}
