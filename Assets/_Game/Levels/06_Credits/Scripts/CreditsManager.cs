using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CreditsManager : MonoBehaviour
{
    Animator _animator = null;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape) && _animator.GetBool("Done")) return;

        DataManager.SceneLoader.LoadScene("MainMenu");
    }
}