using UnityEngine;
using Utility.Audio.Clips.Base;

public class AudioOnClick : MonoBehaviour, IInteractable
{
    [SerializeField] private SfxBase _audioClip = null;

    public void OnLeftClick(Vector3 mousePosition) {
        if (_audioClip != null) {
            _audioClip.Play(mousePosition);
        }
    }

    public void OnRightClick(Vector3 mousePosition) {
    }

    public void OnLeftClick() {
    }

    public void OnRightClick() {
    }

    public void OnHoverEnter() {
    }

    public void OnHoverExit() {
    }
}