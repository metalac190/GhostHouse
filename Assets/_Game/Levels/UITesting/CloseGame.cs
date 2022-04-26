using UnityEngine;

public class CloseGame : MonoBehaviour
{
    public void Exit() {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void ReturnToMenu() {
        DataManager.SceneLoader.LoadScene(0);
    }
}