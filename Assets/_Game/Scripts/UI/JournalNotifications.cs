using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mechanics.Level_Mechanics;
using UnityEngine;
using Yarn.Unity;

public class JournalNotifications : MonoBehaviour
{
    [SerializeField] private List<GameObject> _hintObjects = new List<GameObject>();

    private DialogueRunner _dialogueRunner;
    private DialogueRunner DialogueRunner {
        get {
            if (_dialogueRunner == null) {
                _dialogueRunner = FindObjectOfType<DialogueRunner>();
            }
            return _dialogueRunner;
        }
    }

    private bool _notificationInLine;

    private void OnEnable() {
        foreach (JournalHint hint in _hintObjects.Select(obj => obj.GetComponents<JournalHint>()).SelectMany(hints => hints)) {
            hint.InteractableShowAfter.OnInteractionFinish += OnNotification;
        }
    }

    private void OnDisable() {
        foreach (JournalHint hint in _hintObjects.Select(obj => obj.GetComponents<JournalHint>()).SelectMany(hints => hints)) {
            hint.InteractableShowAfter.OnInteractionFinish -= OnNotification;
        }
    }

    private void OnNotification() {
        if (_notificationInLine) return;
        StartCoroutine(NotificationWait());
    }

    private IEnumerator NotificationWait() {
        _notificationInLine = true;
        while (DialogueRunner.IsDialogueRunning) {
            yield return null;
        }
        ModalWindowController.Singleton.AddJournalNotification();
        _notificationInLine = false;
    }
}