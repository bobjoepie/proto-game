using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Exp Item", menuName = "Item/Exp Item"), Serializable]
public class ExpSO : PickupSO
{
    public int amount;

    private void Reset()
    {
        amount = 1;
    }
}
