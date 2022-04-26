using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TextBubbleController : MonoBehaviour
{
    public static TextBubbleController Instance;

    [SerializeField] private Transform _transform = null;
    [SerializeField] private Transform _characterParent = null;

    private static string[] _validNames = new[] { "Morgan", "Valerie", "Jaqueline", "Nathan" };

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        Disable();
    }

    public void SetCharacter(string character) {
        if (_validNames.Contains(character)) {
            var obj = FindCharacterTransform(character);
            if (obj != null) {
                SetPosition(obj);
            }
        }
    }

    public void SetPosition(Transform other) {
        _transform.gameObject.SetActive(true);
        _transform.position = other.position;
    }

    public void Disable() {
        _transform.gameObject.SetActive(false);
        _transform.position = Vector3.zero;
    }

    private Transform FindCharacterTransform(string characterName) {
        return _characterParent.Cast<Transform>().FirstOrDefault(child => child.gameObject.activeSelf && child.gameObject.name.Contains(characterName));
    }
}