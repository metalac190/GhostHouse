using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class InteractableYarn : InteractableBase
{
    static DialogueRunner _dialogueRunner;
    static DialogueRunner DialogueRunner
    {
        get
        {
            if (_dialogueRunner == null)
            {
                _dialogueRunner = FindObjectOfType<DialogueRunner>();
            }
            return _dialogueRunner;
        }
    }

    [SerializeField]
    string yarnNode = "";

    public override void OnLeftClick()
    {
        DialogueRunner.StartDialogue(yarnNode);
    }
}