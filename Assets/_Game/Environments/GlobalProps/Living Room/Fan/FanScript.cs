using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanScript : MonoBehaviour
{
    [SerializeField] public bool _powered = false;

    private void Start()
    {
        if (_powered) GetComponent<Animator>().SetTrigger("power");
    }

    public void StartFan()
    {
        GetComponent<Animator>().SetTrigger("power");
        _powered = true;
    }
}
