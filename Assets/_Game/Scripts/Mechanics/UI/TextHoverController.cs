using TMPro;
using UnityEngine;

namespace Mechanics.UI
{
    public class TextHoverController : MonoBehaviour
    {
        public static TextHoverController Singleton;

        [SerializeField] private GameObject _parent = null;
        [SerializeField] private TextMeshProUGUI _text = null;

        private bool _hovering;

        private void Awake() {
            Singleton = this;
        }

        private void Start() {
            EndHover();
        }

        private void Update() {
            if (_hovering) {
                var pos = Input.mousePosition;
                _parent.transform.position = pos;
            }
        }

        public void StartHover(string text) {
            if (_parent != null) _parent.SetActive(true);
            if (_text != null) _text.text = text;
            _hovering = true;
        }

        public void EndHover() {
            if (_parent != null) _parent.SetActive(false);
            if (_text != null) _text.text = "";
            _hovering = false;
        }
    }
}