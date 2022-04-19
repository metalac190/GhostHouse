using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JournalNotifications : MonoBehaviour
{
    [SerializeField] private List<GameObject> _hintObjects = new List<GameObject>();

    private void OnEnable() {
        foreach (JournalHint hint in _hintObjects.Select(obj => obj.GetComponents<JournalHint>()).SelectMany(hints => hints)) {
            hint.InteractableShowAfter.OnInteracted += OnNotification;
        }
    }

    private void OnDisable() {
        foreach (JournalHint hint in _hintObjects.Select(obj => obj.GetComponents<JournalHint>()).SelectMany(hints => hints)) {
            hint.InteractableShowAfter.OnInteracted += OnNotification;
        }
    }

    private static void OnNotification() {
        ModalWindowController.Singleton.AddJournalNotification();
    }
}
