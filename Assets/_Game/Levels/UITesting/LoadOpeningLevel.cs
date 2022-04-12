using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadOpeningLevel : MonoBehaviour
{
    public void LoadSandbox()
    {
        DataManager.Instance.OnNewGame();
        SceneManager.LoadScene("Sandbox");
    }

    public void LoadSpring()
    {
        DataManager.Instance.OnNewGame();
        SceneManager.LoadScene("Spring");
    }
}
