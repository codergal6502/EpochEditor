using CommandLine;

namespace EpochEditor.Console.CommandOptions {
    [Verb("write", HelpText = "Overwrite the SRM file.")]
    internal class WriteOptions : BaseOptions {
        [Option('c', "character", Required = false, HelpText = "The index of the character to write to.", SetName = "Data")]
        public required int Character { get; set; }

        [Option('p', "property", Required = false, HelpText = "The specific character property to write to.", SetName = "Data")]
        public required String Property { get; set; }

        [Option('v', "value", Required = true, HelpText = "The value to write.")]
        public required String Value { get; set; }
    }
}
