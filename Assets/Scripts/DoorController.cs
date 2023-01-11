using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject door;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            door.gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }
}
