using UnityEngine;

public class LoadGame : MonoBehaviour
{
    public void LoadLevel() {
        var scene = DataManager.Instance.level;
        DataManager.Instance.OnContinueGame();
        DataManager.SceneLoader.LoadScene(scene);
    }
}
