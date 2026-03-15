namespace BaseRMS.Entities;

public class Translation
{
    public int Id { get; set; }

    public required string EntityType { get; set; }

    public int EntityId { get; set; }

    public required string FieldName { get; set; }

    public required string LanguageCode { get; set; }

    public required string Value { get; set; }

}
