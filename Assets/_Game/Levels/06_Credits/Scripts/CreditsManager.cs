using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        if (_animator.GetBool("Done")) return;

        SceneManager.LoadScene("MainMenu");
    }
}