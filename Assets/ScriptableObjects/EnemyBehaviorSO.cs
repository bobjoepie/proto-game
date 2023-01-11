public class EnemyBehaviorSO : BehaviorSO
{
    public string name;
    public string description;
    public BehaviorType behaviorType;
}

public enum BehaviorType
{
    None,
    Passive,
    Aggressive,
    Wondering,
    Curious
}
