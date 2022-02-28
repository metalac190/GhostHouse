using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadOpeningLevel : MonoBehaviour
{
    public void LoadSandbox()
    {
        SceneManager.LoadScene("Sandbox");
    }

    public void LoadSpring()
    {
        SceneManager.LoadScene("Spring");
    }
}
