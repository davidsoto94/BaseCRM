namespace BaseRMS.Entities;

public class FileVersion
{
    public int Id { get; set; }
    public int FileId { get; set; }
    public ApplicationFile File { get; set; }
    public required string StoragePath { get; set; }
    public int VersionNumber { get; set; }
    public DateTime CreatedAt { get; set; }

}
