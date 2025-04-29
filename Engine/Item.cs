namespace Engine;

public class Item (int id, string name, string namePlural)
{
    public int ID { get; set; } = id;
    public string Name { get; set; } = name;
    public string NamePlural { get; set; } = namePlural;
}