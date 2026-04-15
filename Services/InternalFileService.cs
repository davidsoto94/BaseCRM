using BaseRMS.Services.Interfaces;

namespace BaseRMS.Services;

public class InternalFileService : IFileService
{
    public async Task<string> SaveFileAsync(IFormFile file, string folderPath, string? fileName = null)
    {
        var baseDirectory = AppContext.BaseDirectory;
        var fullFolderPath = Path.Combine(baseDirectory, "files", folderPath);

        // Ensure the directory exists
        if (!Directory.Exists(fullFolderPath))
        {
            Directory.CreateDirectory(fullFolderPath);
        }

        if (string.IsNullOrEmpty(fileName))
        {
            fileName = Path.GetFileName(file.FileName);
        }
        var filePath = Path.Combine(fullFolderPath, fileName);

        // Save the file
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return filePath;
    }



    public Task<string> DeleteFileAsync(string fullFilePath)
    {
        if (File.Exists(fullFilePath))
        {
            File.Delete(fullFilePath);
        }
        return Task.FromResult(fullFilePath);
    }

    public async Task<byte[]?> GetFileAsync(string fullFilePath)
    {
        if (!File.Exists(fullFilePath))
        {
            return null;
        }
        var fileBytes = await File.ReadAllBytesAsync(fullFilePath);
        return fileBytes;
    }

    public async Task<Stream?> GetFileStreamAsync(string fullFilePath)
    {
        if (!File.Exists(fullFilePath))
        {
            return null;
        }
        var fileStream = new FileStream(fullFilePath, FileMode.Open, FileAccess.Read);
        return fileStream;
    }
}