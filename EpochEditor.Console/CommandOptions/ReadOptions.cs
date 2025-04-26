using CommandLine;

namespace EpochEditor.Console.CommandOptions {
    [Verb("read", HelpText = "Read the SRM file.")]
    internal class ReadOptions : BaseOptions {
        [Option('s', "slot", Required = true, HelpText = "The index of the slot to read for.", SetName = "Data")]
        public int Slot { get; set; }

        [Option('c', "character", Required = false, HelpText = "The index of the character to read for.", SetName = "Data")]
        public int? Character { get; set; }

        [Option('p', "property", Required = false, HelpText = "The specific character property to read.", SetName = "Data")]
        public String? Property { get; set; }
    }
}
