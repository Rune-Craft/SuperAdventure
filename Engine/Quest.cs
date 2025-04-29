namespace Engine;

public class Quest (int id, string name, string description,
                    int rewardExperiencePoints, int rewardGold)
{
    public int ID { get; set; } = id;
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;
    public int RewardExperiencePoints { get; set; } = rewardExperiencePoints;
    public int RewardGold { get; set; } = rewardGold;
    public Item RewardItem { get; set; }
    public List<QuestCompletionItem> QuestCompletionItems { get; set; } = [];
}