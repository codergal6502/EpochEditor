using CommandLine;

namespace EpochEditor.Console.CommandOptions {
    [Verb("read", HelpText = "Read the SRM file.")]
    internal class ReadOptions : BaseOptions {
        [Option('a', "all", Required = false, Default = true, HelpText = "Read all properties.")]
        public bool All { get; set; }

        [Option('p', "property", Required = false, HelpText = "The specific character property to read.")]
        public String? Property { get; set; }
    }
}
