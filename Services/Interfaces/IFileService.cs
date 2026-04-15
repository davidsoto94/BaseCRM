namespace BaseRMS.Services.Interfaces;

public interface IFileService
{
    Task<string> SaveFileAsync(IFormFile file, string folderPath, string? fileName = null);

    /// <summary>
    /// Gets file content as bytes
    /// </summary>
    Task<byte[]?> GetFileAsync(string fullFilePath);

    /// <summary>
    /// Gets file content as stream (better for large files)
    /// </summary>
    Task<Stream?> GetFileStreamAsync(string fullFilePath);

    Task<string> DeleteFileAsync(string fullFilePath);
}
