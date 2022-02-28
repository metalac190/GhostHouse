using System.Collections.Generic;
using UnityEngine;
using Utility.Buttons;

namespace Mechanics.Level_Mechanics
{
    [CreateAssetMenu(fileName = "NewInteractable", menuName = "Interactions/Interactable")]
    public class Interactable : ScriptableObject
    {
        //public variables that designers can edit
        [Header("Interaction Informantion")]
        [SerializeField] private string _interactableName = "Default Name";
        [SerializeField, TextArea] private string _interactableDescription = "Default Description";
        [SerializeField] public bool _interacted = false;
        [SerializeField] private bool _canInteractMultipleTimes = false;

        public bool CanInteract => !_interacted || _canInteractMultipleTimes;

        private List<InteractableResponse> _interactableResponses = new List<InteractableResponse>();

        public void Raise(InteractableResponse response) {
            _interactableResponses.Add(response);
        }

        public void Unraise(InteractableResponse response) {
            _interactableResponses.Remove(response);
        }

        [Button(Spacing = 10)]
        public void Interact() {
            //foreach (var response in _interactableResponses) {
            //   response.Invoke();
            //}

            //The same for loop as before, but this one goes backwards to make sure that deleting/removing a interactableResponse doesn't
            //cause any errors.
            _interacted = true;
            SaveInteraction();
            for (int i = _interactableResponses.Count - 1; i >= 0; i--) {
                _interactableResponses[i].Invoke();
            }
        }

        public void SaveInteraction() {
            DataManager.Instance.SetInteraction(_interactableName, _interacted);
        }

        public void LoadInteraction() {
            _interacted = DataManager.Instance.GetInteraction(_interactableName);
        }
    }
}