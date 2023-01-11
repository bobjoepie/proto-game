public class LevelUpgradeSO : UpgradeSO
{
    public string name;
    public string description;
    public LevelUpgradeType upgradeType;
}

public enum LevelUpgradeType
{
    Stat,
    Skill,
    ItemUpgrade
}