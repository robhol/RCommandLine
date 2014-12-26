namespace ExampleApplication
{
    using RCommandLine;
    using RCommandLine.Attributes;

    [
    HasCommand(typeof(ReadOptions), Description = "Read a file"), //Name default: read
    HasCommand(typeof(WriteOptions), Description = "Write a file"), //Name default: write
    ] 
    class ExampleOptions
    {
        public abstract class FileOptions : ExampleOptions
        {
            [Argument(0, Description = "The file to act upon")] //Name default: Path
            public string Path { get; set; }

            [Flag('F', Description = "The format to use in a file operation"), Optional(Default = "foo")] //Name default: format (--format)
            public string Format { get; set; }
        }

        [Usage(@"""input file.txt""", "Reads a file using the default settings for format and chunk size.", Order = 0)]
        [Usage(@"""input file.txt"" -cF 4096 ASCII", "Reads a file using format ASCII and chunk size 4096.", Order = 1)]
        [Remark("If the chunk size is not specified, it will depend on the format. For more details, consult the manual.", Order = 0)]
        public class ReadOptions : FileOptions
        {
            [Flag('c'), Optional(Default = 2048)] // (--chunk-size)
            public int ChunkSize { get; set; }
        }

        [Usage(@"""output file.txt"" -dF XML", "Writes a file in XML format, including debug information.", Order = 0)]
        [Usage(@"""output file.txt""", "Writes a file with standard settings.", Order = 1)]
        [Remark("If the chunk size is not specified, it will depend on the format. For more details, consult the manual.", Order = 0)]
        [Remark("If the file exists, it will be overwritten without warning.", Order = 1)]
        public class WriteOptions : FileOptions
        {
            [Flag('d', Description = "Whether or not to include debug information in the output")]
            public bool DebugInfo { get; set; }
        }

    }
}
