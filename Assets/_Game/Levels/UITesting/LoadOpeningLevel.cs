using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadOpeningLevel : MonoBehaviour
{
    public void LoadSandbox()
    {
        DataManager.Instance.ResetData();
        SceneManager.LoadScene("Sandbox");
    }

    public void LoadSpring()
    {
        DataManager.Instance.ResetData();
        SceneManager.LoadScene("Spring");
    }
}
