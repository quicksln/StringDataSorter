using StringDataSorter.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringDataSorter.Core
{
    /// <summary>
    /// Manages file operations such as creating, reading, and writing data to files.
    /// </summary>
    public class FileDataManager : IDataManager
    {
        /// <summary>
        /// Creates a file with random data until the specified file size is reached.
        /// </summary>
        public async Task CreateDataAsync(string[] dataSampler, string filePath, long fileSize, CancellationToken cancellationToken = default)
        {
            Random random = new Random();
            int newLineByteSize = Environment.NewLine.Length;
            long totalBytesWritten = 0;
            long targetFileSizeBytes = fileSize * 1024L * 1024L;

            HasEnoughSpaceForFile(filePath, targetFileSizeBytes);
            CreateFolderPathIfNotExists(filePath);

            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.ASCII))
            {
                var sb = new StringBuilder(capacity: 8192);
                while (totalBytesWritten < targetFileSizeBytes)
                {
                    int randomNumber = random.Next(1, int.MaxValue);
                    string line = $"{randomNumber}. {dataSampler[random.Next(dataSampler.Length)]}";

                    sb.AppendLine(line);
                    totalBytesWritten += line.Length + newLineByteSize;

                    if (sb.Length > 8192)
                    {
                        await writer.WriteAsync(sb, cancellationToken);
                        sb.Clear();
                    }
                }

                if (sb.Length > 0)
                {
                    await writer.WriteAsync(sb, cancellationToken);
                    sb.Clear();
                }
            }
        }

        /// <summary>
        /// Reads all lines from a file asynchronously.
        /// </summary>
        public async Task<string[]> ReadDataAsync(string filePath, CancellationToken cancellationToken = default)
        {
            const int bufferSize = 65536;

            using var reader = new StreamReader(filePath, Encoding.ASCII, detectEncodingFromByteOrderMarks: false, bufferSize: bufferSize);

            List<string> lines = new List<string>();
            string? line;
            while ((line = await reader.ReadLineAsync()) is not null)
            {

                lines.Add(line);
            }

            return lines.ToArray();
        }

        /// <summary>
        /// Writes an array of strings to a file asynchronously.
        /// </summary>
        public async Task WriteDataAsync(string filePath, string[] dataToWrite, CancellationToken cancellationToken = default)
        {
            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.ASCII))
            {
                StringBuilder sb = new StringBuilder(capacity: 8192);

                foreach (var line in dataToWrite)
                {
                    sb.AppendLine(line);

                    if (sb.Length > 8192)
                    {
                        await writer.WriteAsync(sb, cancellationToken);
                        sb.Clear();
                    }
                }

                if (sb.Length > 0)
                {
                    await writer.WriteAsync(sb, cancellationToken);
                    sb.Clear();
                }
            }
        }

        #region Private mthods

        private void CreateFolderPathIfNotExists(string? filePath)
        {
            string? folderPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(folderPath) && !string.IsNullOrEmpty(folderPath))
            {
                Directory.CreateDirectory(folderPath!);
            }
        }

        public void HasEnoughSpaceForFile(string? filePath, long fileSizeBytes)
        {

            string? rootPath = Path.GetPathRoot(filePath);

            if (string.IsNullOrEmpty(rootPath))
            {
                throw new ArgumentException("Invalid file path. Could not find drive.");
            }

            DriveInfo drive = new DriveInfo(rootPath);

            if (drive.AvailableFreeSpace <= fileSizeBytes)
            {
                throw new IOException("Not enough space on disk to create file.");
            }
        }

        #endregion
    }
}
