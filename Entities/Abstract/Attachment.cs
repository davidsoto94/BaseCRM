namespace BaseRMS.Entities.Abstract;

public abstract class Attachment<IEntity>
    where IEntity : class

{
    public int Id { get; set; }
    public int EntityId { get; set; }
    public int FileId { get; set; }
    public DateTime UploadedAt { get; set; }
    public required ApplicationFile File { get; set; }
    public required IEntity Entity { get; set; }
    public bool IsDeleted { get; set; }
}
