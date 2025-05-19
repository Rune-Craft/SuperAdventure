namespace Engine;

public class Item (int id, string name, string namePlural, int price)
{
    public int ID { get; set; } = id;
    public string Name { get; set; } = name;
    public string NamePlural { get; set; } = namePlural;
    public int Price { get; set; } = price;
}
