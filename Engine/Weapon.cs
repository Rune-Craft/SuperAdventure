namespace Engine;

public class Weapon (int id, string name, string namePlural, int minimumDamage,
                   int maximumDamage) : Item(id, name, namePlural)
{
    public int MinimumDamage { get; set; } = minimumDamage;
    public int MaximumDamage { get; set; } = maximumDamage;
}