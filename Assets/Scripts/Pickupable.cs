using System;
using UnityEngine;

public class Pickupable : MonoBehaviour
{
    public PickupSO pickupObj;
    public int amount = 1;

    private void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Item");
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.root.TryGetComponent<EntityController>(out var entity))
        {
            if (entity.PickUpItem(this))
            {
                Destroy(this.gameObject);
            }
        }
    }
}