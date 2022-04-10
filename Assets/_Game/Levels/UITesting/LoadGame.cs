using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
    public void LoadLevel() {
        var scene = DataManager.Instance.level;
        DataManager.Instance.OnContinueGame();
        SceneManager.LoadScene(scene);
    }
}
