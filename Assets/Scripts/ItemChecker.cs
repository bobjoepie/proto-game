using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemChecker : MonoBehaviour
{
    public ItemSO itemToCheck;
    public ItemCheckerAction action;
    public Vector2 teleportDestination;
    public List<Sprite> sprites;
    public int spriteIndex;
    //TODO: modular actions when matched item
    public void MatchedItem(PlayerControllerOld player)
    {
        //TeleportOther(player.gameObject);
        switch (action)
        {
            case ItemCheckerAction.Teleport:
                ChangeScene("Stage1_Cutscene");
                break;
            case ItemCheckerAction.ChangeSprite:
                ChangeSprite();
                break;
            default:
                break;
        }
        
    }

    public void TeleportOther(GameObject actor) 
    {
        actor.transform.position = teleportDestination;
    }

    public void ChangeScene(string sceneName)
    {
        //Object.FindObjectOfType<AudioManagerOld>().backgroundMusic.Stop();
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void ChangeSprite()
    {
        spriteIndex++;
        if (spriteIndex >= sprites.Count) return;
        var sprite = sprites[spriteIndex];
        var spriteObj = GetComponentInChildren<SpriteRenderer>();
        spriteObj.sprite = sprite;
    }
}

public enum ItemCheckerAction
{
    Teleport,
    ChangeSprite
}