using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    public GameObject itemToSpawn;

    private bool IsQuitting = false;

    private void OnDestroy()
    {
        // TODO: Buggy, move this script to interface and don't use OnDestroy instantiation
        SpawnItem(itemToSpawn);
    }

    private void OnApplicationQuit()
    {
        IsQuitting = true;
    }

    private void SpawnItem(GameObject item)
    {
        if (!IsQuitting)
        {
            Instantiate(itemToSpawn, transform.position, Quaternion.identity);
        }
    }

}
