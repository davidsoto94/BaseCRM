namespace BaseRMS.Services.Interfaces;

public interface IFileService
{
    public Task<string> SaveFileAsync(IFormFile file, string folderPath, string? fileName = null);

    public Task<string> DeleteFileAsync(string fullFilePath);
}
