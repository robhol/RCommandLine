namespace ExampleApplication
{
    using RCommandLine;
    using RCommandLine.Attributes;

    [
    HasCommand(typeof(ReadOptions), Description = "Read a file"), //Name default: read
    HasCommand(typeof(WriteOptions), Description = "Write a file") //Name default: write
    ] 
    class ExampleOptions
    {
        public abstract class FileOptions : ExampleOptions
        {
            [Argument(0, Description = "The file to act upon")] //Name default: Path
            public string Path { get; set; }

            [Flag('F', longName: "", Description = "The format to use in a file operation"), Optional(Default = "foo")] //Name default: format (--format)
            public string Format { get; set; }
        }

        public class ReadOptions : FileOptions
        {
            [Flag('c'), Optional(Default = 2048)] // (--chunk-size)
            public int ChunkSize { get; set; }
        }

        public class WriteOptions : FileOptions
        {
            [Flag('d', Description = "Whether or not to include debug information in the output")]
            public bool DebugInfo { get; set; }
        }

    }
}
