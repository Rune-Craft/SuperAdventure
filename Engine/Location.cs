namespace Engine;

public class Location (int id, string name, string description,
                     Item itemRequiredToEnter = null,
                     Quest questAvailableHere = null,
                     Monster monsterLivingHere = null)
{
    public int ID { get; set; } = id;
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;
    public Item ItemRequiredToEnter { get; set; } = itemRequiredToEnter;
    public Quest QuestAvailableHere { get; set; } = questAvailableHere;
    public Monster MonsterLivingHere { get; set; } = monsterLivingHere;
    public Location LocationToNorth { get; set; }
    public Location LocationToEast { get; set; }
    public Location LocationToSouth { get; set; }
    public Location LocationToWest { get; set; }
}