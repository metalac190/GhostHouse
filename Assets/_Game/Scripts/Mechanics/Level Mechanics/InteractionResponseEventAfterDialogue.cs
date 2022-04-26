using System.Collections;
using System.Collections.Generic;
using Mechanics.Level_Mechanics;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

public class InteractionResponseEventAfterDialogue : MonoBehaviour
{
    [SerializeField] private Interactable _interactable = null;
    [SerializeField] private UnityEvent _event = new UnityEvent();

    private DialogueRunner _dialogueRunner;
    private DialogueRunner DialogueRunner {
        get {
            if (_dialogueRunner == null) {
                _dialogueRunner = FindObjectOfType<DialogueRunner>();
            }
            return _dialogueRunner;
        }
    }

    private void OnEnable() {
        if (_interactable == null) return;
        _interactable.OnInteractionFinish += StartInvoke;
    }

    private void OnDisable() {
        if (_interactable == null) return;
        _interactable.OnInteractionFinish -= StartInvoke;
    }

    private void StartInvoke() {
        StartCoroutine(WaitInvoke());
    }

    private IEnumerator WaitInvoke() {
        while (DialogueRunner.IsDialogueRunning) {
            yield return null;
        }
        InvokeEvent();
    }

    private void InvokeEvent() {
        _event.Invoke();
    }
}