using Mechanics.Level_Mechanics;
using UnityEngine;

public class AfterDialogueInteraction : MonoBehaviour
{
    public static AfterDialogueInteraction Singleton;
    private StoryInteractable _nextInteraction;

    private void Awake() {
        Singleton = this;
    }

    public void SetNextInteraction(StoryInteractable interaction) {
        _nextInteraction = interaction;
    }

    public void OnDialogueFinished() {
        if (_nextInteraction == null) return;
        _nextInteraction.OnLeftClick(_nextInteraction.transform.position);
        _nextInteraction = null;
    }
}
