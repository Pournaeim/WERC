namespace Model.ToolsModels.Tree
{
    public interface ITmNode
    {
        int Id { get; set; }
        int? ParentId { get; set; }
        string Name { get; set; }
        string AdditionalData { get; set; }
    }
}
