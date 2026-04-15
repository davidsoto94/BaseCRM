namespace BaseRMS.Entities;


public class ApplicationFile
{
    public int Id { get; set; }
    public string Name { get; set; } = ""; 
    public int CurrentVersionId { get; set; }
    public required FileVersion CurrentVersion { get; set; }
    public ICollection<FileVersion> Versions { get; set; } = new List<FileVersion>();
}
