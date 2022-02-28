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
        [SerializeField] public string _interactableName = "Default Name";
        [SerializeField, TextArea] public string _interactableDescription = "Default Description";
        [SerializeField] public bool interacted = false;

        [Header("Modal Window Information")]
        [SerializeField] public string modalWindowDisplayText = "";
        [SerializeField] public bool modalWindowDisplayImage = false;
        [SerializeField] public Sprite modalWindowImageToDisplay = null;
        [SerializeField] public bool hasCancelButton = true;
        [SerializeField] public string mainInteractionButtonText = "";
        [SerializeField] public string altInteractionButtonText = "";


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
            for (int i = _interactableResponses.Count - 1; i >= 0; i--)
            {
                _interactableResponses[i].Invoke();
            }
        }
    }
}