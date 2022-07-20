using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemChecker : MonoBehaviour
{
    public ItemSO itemToCheck;
    public Vector2 teleportDestination;
    //TODO: modular actions when matched item
    public void MatchedItem(PlayerController player)
    {
        //TeleportOther(player.gameObject);
        ChangeScene("Stage1_Cutscene");
    }

    public void TeleportOther(GameObject actor) 
    {
        actor.transform.position = teleportDestination;
    }

    public void ChangeScene(string sceneName)
    {
        //Object.FindObjectOfType<AudioManager>().backgroundMusic.Stop();
        SceneManager.LoadSceneAsync(sceneName);
    }
}
