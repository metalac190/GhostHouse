using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(int sceneIndex) {
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}
