using CommandLine;

namespace EpochEditor.Console.CommandOptions {
    [Verb("write", HelpText = "Overwrite the SRM file.")]
    internal class WriteOptions : BaseOptions {
        [Option('s', "slot", Required = false, HelpText = "The index of the slot to write to.", SetName = "Data")]
        public required int Slot { get; set; }

        [Option('c', "character", Required = false, HelpText = "The index of the character to write to.", SetName = "Data")]
        public required int Character { get; set; }

        [Option('p', "property", Required = false, HelpText = "The specific character property to write to.", SetName = "Data")]
        public required String Property { get; set; }

        [Option('v', "value", Required = false, HelpText = "The value to write.")]
        public required String Value { get; set; }
    }
}
