namespace Engine;

public class HealingPotion (int id, string name, string namePlural,
                           int amountToHeal, int price) : Item(id, name, namePlural, price)
{
    public int AmountToHeal { get; set; } = amountToHeal;
}
