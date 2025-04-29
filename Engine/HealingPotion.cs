namespace Engine;

public class HealingPotion (int id, string name, string namePlural,
                           int amountToHeal) : Item(id, name, namePlural)
{
    public int AmountToHeal { get; set; } = amountToHeal;
}