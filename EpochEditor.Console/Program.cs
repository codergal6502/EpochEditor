using System.ComponentModel.DataAnnotations;
using CommandLine;
using EpochEditor.Console.CommandOptions;
using EpochEditor.SramUtilities;

namespace EpochEditor.Console.CommandOptions {
}
internal class Program {

    private static void Main(string[] args) {
        Parser
            .Default
            .ParseArguments<ReadOptions, WriteOptions>(args)
            .WithParsed<ReadOptions>(o => {
                Byte[] bytes = File.ReadAllBytes(o.FilePath);
                SramReader sr = new SramReader(bytes);
            })
            .WithNotParsed(errors => {

            });
    }
}