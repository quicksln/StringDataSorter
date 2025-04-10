namespace StringDataSorter.Core.Interfaces
{
    public interface IDataManager
    {
        Task CreateDataAsync(string[] dataToWrite, string filePath, long fileSize, CancellationToken cancellationToken = default);
        Task<string[]> ReadDataAsync(string filePath, CancellationToken cancellationToken = default);
        Task WriteDataAsync(string filePath, string[] dataToWrite, CancellationToken cancellationToken = default);
    }
}
