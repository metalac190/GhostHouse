using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXTrigger : MonoBehaviour
{
    public VisualEffect _vfx;

    private void Start()
    {
        SetVFXActive(0);
    }

    public void SetVFXActive(int active)
    {
        if (active == 1)
        {
            _vfx.SendEvent("Start");
        }
        else
        {
            _vfx.SendEvent("Stop");
        }
    }
}
