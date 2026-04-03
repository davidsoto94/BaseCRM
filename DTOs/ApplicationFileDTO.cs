namespace BaseRMS.DTOs;

public class ApplicationFileDTO
{
    public required string FileName { get; set; }
    public required IFormFile File { get; set; }
}
