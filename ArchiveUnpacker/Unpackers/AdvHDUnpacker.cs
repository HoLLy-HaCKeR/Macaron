/*
Program Architecture & Framework:    @HoLLy-HaCKeR
Archive Format and Engine Reversing: @Azukee
*/
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ArchiveUnpacker.Framework;
using ArchiveUnpacker.Framework.ExtractableFileTypes;

namespace ArchiveUnpacker.Unpackers
{
    public class AdvHDUnpacker : IUnpacker
    {
        public IEnumerable<IExtractableFile> LoadFiles(string gameDirectory) => GetArchivesFromGameFolder(gameDirectory).SelectMany(LoadFilesFromArchive);

        public IEnumerable<IExtractableFile> LoadFilesFromArchive(string inputArchive)
        {
            using (var fs = File.OpenRead(inputArchive)) {
                using (var br = new BinaryReader(fs)) {
                    int count = br.ReadInt32();
                    int entrySize = br.ReadInt32() + 8;

                    for (int i = 0; i < count; i++) {
                        uint size = (uint) (br.ReadUInt32() + entrySize);
                        uint offset = (uint) (br.ReadUInt32() + entrySize);
                        StringBuilder path = new StringBuilder();
                        while (true) {
                            char c;
                            if ((c = br.ReadChar()) == 0)
                                break;
                            br.ReadByte(); //skip 00 if first char isn't null
                            path.Append(c);
                        }

                        br.ReadByte(); // unused byte
                        yield return new FileSlice(path.ToString(), offset, size, inputArchive);
                    }
                }
            }
        }

        public static bool IsGameFolder(string folder) => Directory.GetFiles(folder, "*.arc").Length > 0 && IsAdvHDGame(folder);

        private IEnumerable<string> GetArchivesFromGameFolder(string gameDirectory) => Directory.GetFiles(gameDirectory, "*.arc");

        private static bool IsAdvHDGame(string gameDirectory)
        {
            foreach (string file in Directory.GetFiles(gameDirectory, "*.exe"))
                if (System.Diagnostics.FileVersionInfo.GetVersionInfo(file).FileDescription == "ADVPlayerHD")
                    return true;

            return false;
        }
    }
}