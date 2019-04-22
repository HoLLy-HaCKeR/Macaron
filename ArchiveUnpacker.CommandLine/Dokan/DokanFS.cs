using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;
using ArchiveUnpacker.Core;
using DokanNet;
using FileAccess = DokanNet.FileAccess;

namespace ArchiveUnpacker.CommandLine.Dokan
{
    public partial class DokanFS : IDokanOperations
    {
        private readonly IList<IExtractableFile> files;
        private Dictionary<string, object> openedFiles = new Dictionary<string, object>();

        public DokanFS(IList<IExtractableFile> files)
        {
            this.files = files;
        }

        #region getting info (volume, paths)

        public NtStatus GetVolumeInformation(out string volumeLabel, out FileSystemFeatures features, out string fileSystemName, out uint maximumComponentLength, DokanFileInfo info)
        {
            volumeLabel = "Macaron Archive";
            features = FileSystemFeatures.ReadOnlyVolume;
            fileSystemName = "MACARON";
            maximumComponentLength = 256;
            return NtStatus.Success;
        }

        public NtStatus FindFiles(string fileName, out IList<FileInformation> files, DokanFileInfo info)
        {
            Console.WriteLine("Listing files in: " + fileName);

            files = new List<FileInformation>();
            files.Add(new FileInformation {
                FileName = "macaron_test.dat",
                Length = 1337,
                Attributes = FileAttributes.ReadOnly,
            });

            return NtStatus.Success;
        }

        public NtStatus GetFileInformation(string fileName, out FileInformation fileInfo, DokanFileInfo info)
        {
            fileInfo = new FileInformation {
                FileName = fileName,
                Length = 1337,
                Attributes = FileAttributes.ReadOnly,
            };

            return NtStatus.Success;
        }

        #endregion

        public void Cleanup(string fileName, DokanFileInfo info)
        {
            // TODO: what's this?
        }

        public void CloseFile(string fileName, DokanFileInfo info)
        {
            Console.WriteLine("[-] Closing file: " + fileName);
            openedFiles.Remove(fileName);
        }

        public NtStatus ReadFile(string fileName, byte[] buffer, out int bytesRead, long offset, DokanFileInfo info)
        {
            throw new NotImplementedException();
        }

        public NtStatus Mounted(DokanFileInfo info) => NtStatus.Success;
        public NtStatus Unmounted(DokanFileInfo info) => NtStatus.Success;

        public NtStatus GetDiskFreeSpace(out long freeBytesAvailable, out long totalNumberOfBytes, out long totalNumberOfFreeBytes, DokanFileInfo info)
        {
            totalNumberOfFreeBytes = freeBytesAvailable = 0;
            totalNumberOfBytes = 1024*1024;

            return NtStatus.Success;
        }

        #region operations we don't implement
        public NtStatus CreateFile(string fileName, FileAccess access, FileShare share, FileMode mode, FileOptions options, FileAttributes attributes, DokanFileInfo info) => DokanResult.Success;
        public NtStatus DeleteDirectory(string fileName, DokanFileInfo info) => DokanResult.NotImplemented;
        public NtStatus DeleteFile(string fileName, DokanFileInfo info) => DokanResult.NotImplemented;
        public NtStatus MoveFile(string oldName, string newName, bool replace, DokanFileInfo info) => DokanResult.NotImplemented;
        public NtStatus LockFile(string fileName, long offset, long length, DokanFileInfo info) => DokanResult.NotImplemented;
        public NtStatus UnlockFile(string fileName, long offset, long length, DokanFileInfo info) => DokanResult.NotImplemented;
        public NtStatus SetEndOfFile(string fileName, long length, DokanFileInfo info) => DokanResult.NotImplemented;
        public NtStatus SetAllocationSize(string fileName, long length, DokanFileInfo info) => DokanResult.NotImplemented;
        public NtStatus SetFileSecurity(string fileName, FileSystemSecurity security, AccessControlSections sections, DokanFileInfo info) => DokanResult.NotImplemented;
        public NtStatus SetFileAttributes(string fileName, FileAttributes attributes, DokanFileInfo info) => DokanResult.NotImplemented;
        public NtStatus SetFileTime(string fileName, DateTime? creationTime, DateTime? lastAccessTime, DateTime? lastWriteTime, DokanFileInfo info) => DokanResult.NotImplemented;

        public NtStatus FindStreams(string fileName, out IList<FileInformation> streams, DokanFileInfo info)
        {
            streams = new FileInformation[0];
            return DokanResult.NotImplemented;
        }

        public NtStatus GetFileSecurity(string fileName, out FileSystemSecurity security, AccessControlSections sections, DokanFileInfo info)
        {
            security = null;
            return DokanResult.NotImplemented;
        }

        public NtStatus WriteFile(string fileName, byte[] buffer, out int bytesWritten, long offset, DokanFileInfo info)
        {
            bytesWritten = 0;
            return DokanResult.NotImplemented;
        }

        // may implement these at some point
        public NtStatus FlushFileBuffers(string fileName, DokanFileInfo info) => DokanResult.NotImplemented;

        public NtStatus FindFilesWithPattern(string fileName, string searchPattern, out IList<FileInformation> files, DokanFileInfo info)
        {
            files = new FileInformation[0];
            return DokanResult.NotImplemented;
        }
        #endregion
    }
}
