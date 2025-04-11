using CommandLine;

namespace EpochEditor.Console.CommandOptions {
    [Verb("write", HelpText = "Overwrite the SRM file.")]
    internal class WriteOptions : BaseOptions {
        [Option('p', "property", Required = true, HelpText = "The specific character property to write to.")]
        public String? Property { get; set; }

        [Option('v', "Value", Required = true, HelpText = "The value to write for the property.")]
        public String? Value { get; set; }
    }
}
