using UnityEngine;
using UnityEngine.EventSystems;

namespace Levels.TestScenes.HoverClickFeedback.Scripts
{
    public class ObjectClickerTest : MonoBehaviour
    {
        private Camera _mainCamera;
        private IInteractable _previousInteractable;

        #region Properties

        // Input
        private static bool LeftClick => Input.GetMouseButtonDown(0);
        private static bool RightClick => Input.GetMouseButtonDown(1);

        // Checks if mouse is over UI
        public static bool IsMouseOverUi {
            get {
                var events = EventSystem.current;
                return events != null && events.IsPointerOverGameObject();
            }
        }

        #endregion

        #region Unity Functions

        private void Start() {
            // Store the main camera for a very minor performance increase
            _mainCamera = Camera.main;
        }

        private void Update() {
            RaycastCheck();
        }

        #endregion

        #region Hovering and Clicking

        private void RaycastCheck() {
            // Ignore raycast if mouse is over UI
            if (IsMouseOverUi) {
                ResetHover();
                return;
            }

            // Get mouse position and raycast
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            // Raycast and hit
            if (Physics.Raycast(ray, out var hit)) {
                OnHover(hit);
                if (LeftClick) OnClick(hit, true);
                if (RightClick) OnClick(hit, false);
            }
            // Raycast failed to hit anything
            else {
                ResetHover();
            }

            // Note: While Hovering is also called immediately after OnHoverEnter()
            //_previousInteractable?.WhileHovering();
        }

        private void OnHover(RaycastHit hit) {
            IInteractable interactable = hit.transform.GetComponent<IInteractable>();

            // If a new object is hovered, reset hover to the new object
            if (interactable != _previousInteractable) {
                ResetHover(interactable);
            }
        }

        private void ResetHover(IInteractable newInteractable = null) {
            // Attempt to stop hovering previous object
            _previousInteractable?.OnHoverExit();

            // Attempt to hover new object
            newInteractable?.OnHoverEnter();

            // Set new object as previous object
            _previousInteractable = newInteractable;
        }

        private static void OnClick(RaycastHit hit, bool left) {
            IInteractable interactable = hit.transform.GetComponent<IInteractable>();

            // Interactable clicked
            if (interactable != null) {
                if (left) {
                    interactable.OnLeftClick();
                }
                else {
                    interactable.OnRightClick();
                }
            }
        }

        #endregion
    }
}