using CommandLine;

namespace EpochEditor.Console.CommandOptions {
    internal class BaseOptions {
        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }

        [Option('f', "file", Required = true, HelpText = "Path to the file to read from or write to.")]
        public required String FilePath { get; set; }
    }
}