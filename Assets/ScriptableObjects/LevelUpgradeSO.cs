public class LevelUpgradeSO : UpgradeSO
{
    public string description;
    public LevelUpgradeType upgradeType;
}

public enum LevelUpgradeType
{
    Stat,
    Skill,
    ItemUpgrade
}