using UnityEngine;

namespace Utility
{
    public class Documentation : MonoBehaviour
    {
        [SerializeField] private TextAsset _documentationText = null;

        public string Text => _documentationText != null ? _documentationText.text : "";
    }
}