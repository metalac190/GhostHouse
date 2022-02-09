using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarnPoker : MonoBehaviour
{
    [SerializeField]
    string _instanceName = "";

    [SerializeField]
    string _timelineName = "";

    [SerializeField]
    YarnObject yarnObj = null;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Poke();
        }
    }

    void Poke()
    {
        yarnObj.SetInstance(_instanceName);

        // this won't wait for the animation to finish (like intended),
        // but yarn spinner will and that's the purpose of the function.
        StartCoroutine(yarnObj.Animate(_timelineName, "wait"));
    }
}
