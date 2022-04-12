using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarnPoker : MonoBehaviour
{
    [SerializeField]
    Yarn.Unity.DialogueRunner _dialogueRunner = null;

    [SerializeField]
    string _nodeName = "";

    [SerializeField]
    KeyCode _continueKey = KeyCode.Return;

    void Update()
    {
        if (Input.GetKeyDown(_continueKey))
        {
            Poke();
        }
    }

    void Poke()
    {
        if (_dialogueRunner != null && _nodeName != "")
        {
            _dialogueRunner.StartDialogue(_nodeName);
        }
    }
}
