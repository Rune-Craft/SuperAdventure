namespace Engine;

public class Weapon (int id, string name, string namePlural, int minimumDamage,
                   int maximumDamage, int price) : Item(id, name, namePlural, price)
{
    public int MinimumDamage { get; set; } = minimumDamage;
    public int MaximumDamage { get; set; } = maximumDamage;
}
