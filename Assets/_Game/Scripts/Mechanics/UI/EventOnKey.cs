using UnityEngine;
using UnityEngine.Events;

public class EventOnKey : MonoBehaviour
{
    [SerializeField] private KeyCode _keyCode = KeyCode.Escape;
    [SerializeField] private UnityEvent _event = new UnityEvent();

    private void Update() {
        if (Input.GetKeyDown(_keyCode)) {
            InvokeEvent();
        }
    }

    public void InvokeEvent() {
        _event.Invoke();
    }
}
