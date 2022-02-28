using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Audio.Clips.Base;

public class AudioOnClick : MonoBehaviour, IInteractable
{
    [SerializeField]
    SfxBase _audioClip = null;

    public void OnLeftClick(Vector3 mousePosition)
    {
        OnLeftClick();
    }

    public void OnRightClick(Vector3 mousePosition) { }

    public void OnLeftClick()
    {
        if (_audioClip != null)
        {
            _audioClip.Play();
        }
    }

    public void OnRightClick() { }

    public void OnHoverEnter() { }

    public void OnHoverExit() { }
}