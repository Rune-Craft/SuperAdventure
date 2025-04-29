namespace Engine;

public class PlayerQuest (Quest details)
{
    public Quest Details { get; set; } = details;
    public bool IsCompleted { get; set; } = false;
}