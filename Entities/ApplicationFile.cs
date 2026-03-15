namespace BaseRMS.Entities;


public class ApplicationFile
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public required string Path { get; set; }
}
