using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cutscene1 : MonoBehaviour
{
    public float sceneChangeTime;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TimedSceneChange("Stage1"));
    }

    IEnumerator TimedSceneChange(string sceneName)
    {
        yield return new WaitForSeconds(sceneChangeTime);
        ChangeScene(sceneName);
    }

    // Update is called once per frame
    public void ChangeScene(string sceneName)
    {
        //Object.FindObjectOfType<AudioManager>().backgroundMusic.Stop();
        SceneManager.LoadSceneAsync(sceneName);
    }
}
