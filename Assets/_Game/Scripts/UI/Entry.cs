using Mechanics.Level_Mechanics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Entry : MonoBehaviour
{
    [SerializeField] private Interactable _interactable = null;
    [SerializeField] private Image _image = null;
    [SerializeField] private TextMeshProUGUI _text = null;

    private void OnEnable() {
        if (_interactable == null) return;
        bool unlocked = DataManager.Instance.interactions[_interactable.name];

        if (_image != null) _image.gameObject.SetActive(unlocked);
        if (_text != null) _text.gameObject.SetActive(unlocked);
    }
}
