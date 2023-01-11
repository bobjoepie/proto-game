using System;
using UnityEngine;

[Serializable]
public abstract class ItemSO : PickupSO
{
    public string name;
    [TextArea(3, 5)]
    public string description;
    [Range(0, 1)]
    public float cooldown;
    [Range(1, 10)] public int amount;

    private void Reset()
    {
        amount = 1;
    }
}

public enum ItemType
{
    PassiveItem,
    ActiveItem,
    ConsumableItem,
}

[Serializable]
public abstract class ItemPart
{
    public string label;

    [Header("Properties")] 
    public ItemType itemType;

    public void UpdateValues(GameObject itemInstance, int iterationNum)
    {
        switch (this)
        {
            case PassiveItemPart passiveItem:
                passiveItem.UpdateValues(itemInstance.GetComponentInChildren<PassiveItemController>(), iterationNum);
                break;
        }
    }
}
