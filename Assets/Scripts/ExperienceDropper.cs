using UnityEngine;

public class ExperienceDropper : MonoBehaviour
{
    public GameObject expToSpawn;
    private bool IsQuitting = false;

    private void OnDestroy()
    {
        // TODO: Buggy, move this script to interface and don't use OnDestroy instantiation
        SpawnItem(expToSpawn);
    }

    private void OnApplicationQuit()
    {
        IsQuitting = true;
    }

    private void SpawnItem(GameObject item)
    {
        if (!IsQuitting)
        {
            Instantiate(expToSpawn, transform.position, Quaternion.identity);
        }
    }
}
