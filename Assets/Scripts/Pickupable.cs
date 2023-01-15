using UnityEngine;

public class Pickupable : MonoBehaviour
{
    public PickupSO pickupObj;
    public int amount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponentInChildren<PlayerControllerOld>();
        if (player != null)
        {
            player.Pickup(gameObject, pickupObj);
        }
    }
}