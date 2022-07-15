using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupableCustom : MonoBehaviour
{
    public WeaponSO weaponPickup;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().PickupItem(gameObject, weaponPickup);
        }
    }
}