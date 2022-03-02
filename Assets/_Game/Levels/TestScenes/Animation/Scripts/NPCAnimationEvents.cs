using UnityEngine;

namespace NPC
{
    [RequireComponent(typeof(Animator))]
    public class NPCAnimationEvents : MonoBehaviour
    {
        [SerializeField] GameObject _objectToDrop = null;

        public void DropCube() {
            if (_objectToDrop != null) {
                GameObject droppedObject = Instantiate(_objectToDrop);
            }
        }
    }
}