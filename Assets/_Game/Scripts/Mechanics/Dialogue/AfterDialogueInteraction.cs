using Mechanics.Level_Mechanics;
using UnityEngine;
using Utility.ReadOnly;

public class AfterDialogueInteraction : MonoBehaviour
{
    public static AfterDialogueInteraction Singleton;
    [SerializeField] private bool _debug = false; 
    [SerializeField, ReadOnly] private StoryInteractable _nextInteraction;

    private void Awake() {
        Singleton = this;
    }

    public void SetNextInteraction(StoryInteractable interaction) {
        _nextInteraction = interaction;
        if (_debug) Debug.Log("Set Interaction: " + interaction.Interaction.name, gameObject);
    }

    public void OnDialogueFinished() {
        if (TextBubbleController.Instance != null) {
            TextBubbleController.Instance.Disable();
        }
        if (_nextInteraction == null) {
            if (_debug) Debug.Log("Dialogue Finished.", gameObject);
            return;
        }
        var interaction = _nextInteraction;
        _nextInteraction = null;
        if (_debug) Debug.Log("Dialogue Finished. Cleared Interaction", gameObject);
        interaction.OnLeftClick(interaction.transform.position);
    }
}
