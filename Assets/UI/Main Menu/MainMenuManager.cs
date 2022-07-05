using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuManager : VisualElement
{
    VisualElement MainMenuView;
    public new class UxmlFactory : UxmlFactory<MainMenuManager, UxmlTraits> { }

    public MainMenuManager()
    {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    void OnGeometryChange(GeometryChangedEvent evt)
    {
        MainMenuView = this.Q("MainMenuView");

        MainMenuView?.Q("start-game-button")?.RegisterCallback<ClickEvent>(ev => StartGameScene());
        MainMenuView?.Q("quit-game-button")?.RegisterCallback<ClickEvent>(ev => QuitGame());

        this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    public void StartGameScene()
    {
        GameObject.FindObjectOfType<AudioManager>().audioData.Stop();
        SceneManager.LoadSceneAsync("GameScene");
    }

    public void QuitGame()
    {
        #if UNITY_STANDALONE
            Application.Quit();
        #endif

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}