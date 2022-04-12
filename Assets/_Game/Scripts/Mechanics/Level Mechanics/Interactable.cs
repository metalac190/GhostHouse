using System;
using System.Collections.Generic;
using UnityEngine;
using Utility.Audio.Helper;
using Utility.Buttons;
using Utility.ReadOnly;
using Yarn.Unity;
using Random = UnityEngine.Random;

namespace Mechanics.Level_Mechanics
{
    [CreateAssetMenu(fileName = "NewInteractable", menuName = "Interactions/Interactable")]
    public class Interactable : ScriptableObject
    {
        [SerializeField, TextArea] private string _interactableDescription = "Default Description";

        public string Description => _interactableDescription;

        [Header("Interaction Settings")]
        [SerializeField, Tooltip("Type in the name inside the dialogue yarn file.")]
        private string _dialogeYarnNode = "";
        [SerializeField, Tooltip("Click this if you want the interaction to happen multiple times.")]
        private bool _canInteractMultipleTimes = false;
        [SerializeField, Tooltip("Click this if you want the interaction to unlock an entry in the Journal.")]
        private bool _opensJournalUnlock;
        [SerializeField, Tooltip("Click this if you want to use random dialogue per every interaction.")]
        private bool _useRandomDialogue = false;
        [SerializeField, Tooltip("Type in the names inside the dialogue yarn files that you want to be used.")]
        private List<string> _randomDialoguePool = new List<string>();

        [Header("Spirit Points")]
        [SerializeField, Tooltip("The Spirit Points cost for the interaction")]
        private int _cost = 0;
        [SerializeField, Tooltip("Points allocated to the Sister Ending from this interaction")]
        private int _sisterEndingPoints = 0;
        [SerializeField, Tooltip("Points allocated to the Cousin Ending from this interaction")]
        private int _cousinEndingPoints = 0;
        [SerializeField, Tooltip("Points allocated to the True Ending from this interaction")]
        private int _trueEndingPoints = 0;

        [Header("Other Settings")]
        [SerializeField] private SfxReference _sfxOnInteract = new SfxReference();

        public List<MeshRenderer> ConnectedMeshRenderers { get; set; }
        public List<Animator> ConnectedAnimators { get; set; } = new List<Animator>();

        public int Cost => _cost;

        static DialogueRunner _dialogueRunner;
        static DialogueRunner DialogueRunner {
            get {
                if (_dialogueRunner == null) {
                    _dialogueRunner = FindObjectOfType<DialogueRunner>();
                }
                return _dialogueRunner;
            }
        }


        public bool Interacted => DataManager.Instance.GetInteraction(name);
        public bool CanInteract => !Interacted || _canInteractMultipleTimes;

        private List<InteractableResponseBase> _interactableResponses = new List<InteractableResponseBase>();

        public void Raise(InteractableResponseBase response) {
            _interactableResponses.Add(response);
        }

        public void Unraise(InteractableResponseBase response) {
            _interactableResponses.Remove(response);
        }

        [Button(Spacing = 10)]
        public void Interact() {
            //foreach (var response in _interactableResponses) {
            //   response.Invoke();
            //}

            //The same for loop as before, but this one goes backwards to make sure that deleting/removing a interactableResponse doesn't
            //cause any errors.


            for (int i = _interactableResponses.Count - 1; i >= 0; i--) {
                _interactableResponses[i].Invoke();
            }

            if (_cost > 0) {
                // TODO: Apply Spirit Point Cost
                DataManager.Instance.remainingSpiritPoints -= _cost;
                ModalWindowController.Singleton.PlaySpiritPointSpentSounds(DataManager.Instance.remainingSpiritPoints <= 0);
            }

            _sfxOnInteract.Play();
            DataManager.Instance.SetInteraction(name, true);

            DataManager.Instance.trueEndingPoints += _trueEndingPoints;
            DataManager.Instance.cousinsEndingPoints += _cousinEndingPoints;
            DataManager.Instance.sistersEndingPoints += _sisterEndingPoints;

            if (!string.IsNullOrEmpty(_dialogeYarnNode)) {
                try {
                    DialogueRunner.StartDialogue(_dialogeYarnNode);
                }
                catch (Exception e) {
                    Debug.LogWarning("Invalid Dialogue Yarn Node (" + _dialogeYarnNode + ") connected to " + name);
                }
            }
            else if (_useRandomDialogue) {
                if (_randomDialoguePool.Count == 0) {
                    Debug.LogWarning("The Random Dialogue Pool has no dialogues in it...");
                }
                else {
                    var dialogue = _randomDialoguePool[Random.Range(0, _randomDialoguePool.Count)];
                    try
                    {
                        DialogueRunner.StartDialogue(dialogue);
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning("Invalid Dialogue Yarn Node (" + dialogue + ") connected to " + name);
                    }
                }
            }
        }
    }
}