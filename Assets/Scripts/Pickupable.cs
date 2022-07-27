using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour
{
    public PickupSO pickupObj;
    public int amount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponentInChildren<PlayerController>();
        if (player != null)
        {
            player.Pickup(gameObject, pickupObj);
        }
    }
}