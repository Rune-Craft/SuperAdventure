namespace Engine;

public class LivingCreature (int currentHitPoints, int maximumHitPoints)
{
    public int CurrentHitPoints { get; set; } = currentHitPoints;
    public int MaximumHitPoints { get; set; } = maximumHitPoints;
}