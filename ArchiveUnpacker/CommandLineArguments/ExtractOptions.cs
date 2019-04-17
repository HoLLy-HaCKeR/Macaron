using CommandLine;

namespace ArchiveUnpacker.CliArgs
{
    /// <summary>
    /// Extract all files from an archive or game folder
    /// </summary>
    [Verb("extract", HelpText = "Extract all files from an archive or game folder")]
    public class ExtractOptions
    {
        [Value(0, MetaName = "Game directory", HelpText = "The root directory of the game", Required = true)]
        public string Directory { get; set; }
    }
}
