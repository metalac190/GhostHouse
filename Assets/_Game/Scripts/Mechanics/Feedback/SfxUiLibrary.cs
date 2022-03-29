using UnityEngine;

namespace Mechanics.Feedback
{
    [CreateAssetMenu(menuName = "Sound System/Libraries/Sfx UI Library")]
    public class SfxUiLibrary : ScriptableObject
    {
        #region Buttons

        public void OnHoverOverButton() {
        }

        public void OnHoverOffButton() {
        }

        public void OnClickButtonGeneric() {
        }

        #endregion

        #region Sliders

        public void OnHoverOverSlider() {
        }

        public void OnHoverOffSlider() {
        }

        public void OnStartDraggingSlider() {
        }

        public void OnDragSlider() {
        }

        public void OnStopDraggingSlider() {
        }

        #endregion

        #region Spirit Points

        public void OnSpendSpiritPoint() {
        }

        public void OnNoSpiritPointsLeft() {
        }

        #endregion

        #region Main Menu

        public void OnStartGame() {
        }

        public void OnLoadGame() {
        }

        public void OnQuitGame() {
        }

        #endregion

        #region Journal

        public void OnJournalNotification() {
        }

        public void OnOpenJournal() {
        }

        public void OnCloseJournal() {
        }

        public void OnSwitchJournalPage() {
        }

        #endregion
    }
}