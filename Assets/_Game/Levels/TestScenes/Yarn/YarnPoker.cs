using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarnPoker : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField]
    Yarn.Unity.DialogueRunner _dialogueRunner = null;

    [SerializeField]
    string _nodeName = "";

    [Header("Yarn Object")]
    [SerializeField]
    YarnObject _yarnObj = null;

    [SerializeField]
    string _instanceName = "";

    [SerializeField]
    string _timelineName = "";


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Poke();
        }
    }

    void Poke()
    {
        if (_yarnObj != null)
        {
            if (_instanceName != "")
            {
                _yarnObj.SetInstance(_instanceName);
            }

            if (_timelineName != "")
            {
                _yarnObj.Animate(_timelineName);
                // this object won't wait for the animation to finish (like intended),
                // but yarn spinner will and that's the purpose of the function.
                //StartCoroutine(yarnObj.LockedAnimate(_timelineName));
            }
        }

        if (_dialogueRunner != null && _nodeName != "")
        {
            _dialogueRunner.StartDialogue(_nodeName);
        }
    }
}
