namespace Engine;

public class Monster (int id, string name, int maximumDamage,
                    int rewardExperiencePoints, int rewardGold,
                    int currentHitPoints, int maximumHitPoints) : LivingCreature(currentHitPoints, maximumHitPoints)
{
    public int ID { get; set; } = id;
    public string Name { get; set; } = name;
    public int MaximumDamage { get; set; } = maximumDamage;
    public int RewardExperiencePoints { get; set; } = rewardExperiencePoints;
    public int RewardGold { get; set; } = rewardGold;
    public List<LootItem> LootTable { get; set; } = [];
}