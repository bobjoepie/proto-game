using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour
{
    public ProjectileAttackScriptableObject projectilePickup;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // TODO: change so that player handles picking up if space available rather than destroy on trigger
            collision.gameObject.GetComponent<PlayerController>().PickupItem(gameObject.name, gameObject.GetComponent<SpriteRenderer>(), projectilePickup);
            Destroy(gameObject);
        }
    }
}
