using System.Collections.Generic;
using UnityEngine;
using Utility.Buttons;

namespace Mechanics.Level_Mechanics
{
    [CreateAssetMenu]
    public class Interactable : ScriptableObject
    {
        private List<InteractableResponse> _interactableResponses = new List<InteractableResponse>();

        public void Subscribe(InteractableResponse response) {
            _interactableResponses.Add(response);
        }

        public void Unsubscribe(InteractableResponse response) {
            _interactableResponses.Remove(response);
        }

        [Button(Spacing = 10)]
        public void Interact() {
            foreach (var response in _interactableResponses) {
                response.Invoke();
            }
        }
    }
}