using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CreditsManager : MonoBehaviour
{
    [SerializeField]
    float _speed = 2.5f;

    Animator _animator = null;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _animator.speed = _speed;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            _animator.speed = 1f;
        }
        if (!Input.GetKeyDown(KeyCode.Escape) && _animator.GetBool("Done")) return;

        DataManager.SceneLoader.LoadScene("MainMenu");
    }
}