using CommandLine;

namespace ArchiveUnpacker.CommandLine.CommandLineParsing
{
    [Verb("mount", HelpText = "Mount a game folder as a filesystem")]
    public class DokanOptions
    {
        [Value(0, MetaName = "Game directory", HelpText = "The root directory of the game", Required = true)]
        public string Directory { get; set; }
    }
}
