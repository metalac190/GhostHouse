using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadOpeningLevel : MonoBehaviour
{
    public void LoadSandbox()
    {
        DataManager.Instance.OnNewGame();
        DataManager.SceneLoader.LoadScene("Sandbox");
    }

    public void LoadSpring()
    {
        DataManager.Instance.OnNewGame();
        DataManager.SceneLoader.LoadScene("Spring");
    }
}
