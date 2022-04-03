using UnityEngine;
using Utility.Audio.Helper;

namespace Mechanics.Feedback
{
    [CreateAssetMenu(menuName = "Sound System/Libraries/Sfx UI Library")]
    public class SfxUiLibrary : ScriptableObject
    {
        #region Buttons

        [Header("Buttons")]
        [SerializeField] private SfxReference _hoverOverButton = new SfxReference(true);
        [SerializeField] private SfxReference _hoverOffButton = new SfxReference(true);
        [SerializeField] private SfxReference _clickButtonGeneric = new SfxReference(true);

        public void OnHoverOverButton() {
            _hoverOverButton.Play();
        }

        public void OnHoverOffButton()
        {
            _hoverOffButton.Play();
        }

        public void OnClickButtonGeneric()
        {
            _clickButtonGeneric.Play();
        }

        #endregion

        #region Sliders

        [Header("Sliders")]
        [SerializeField] private SfxReference _hoverOverSlider = new SfxReference(true);
        [SerializeField] private SfxReference _hoverOffSlider = new SfxReference(true);
        [SerializeField] private SfxReference _startDraggingSlider = new SfxReference(true);
        [SerializeField] private SfxReference _dragSlider = new SfxReference(true);
        [SerializeField] private SfxReference _stopDraggingSlider = new SfxReference(true);

        public void OnHoverOverSlider()
        {
            _hoverOverSlider.Play();
        }

        public void OnHoverOffSlider()
        {
            _hoverOffSlider.Play();
        }

        public void OnStartDraggingSlider()
        {
            _startDraggingSlider.Play();
        }

        public void OnDragSlider()
        {
            _dragSlider.Play();
        }

        public void OnStopDraggingSlider()
        {
            _stopDraggingSlider.Play();
        }

        #endregion

        #region Spirit Points

        [Header("Spirit Points")]
        [SerializeField] private SfxReference _spendSpiritPoint = new SfxReference(true);
        [SerializeField] private SfxReference _noSpiritPointsLeft = new SfxReference(true);
        [SerializeField] private SfxReference _interactionWindowOpen = new SfxReference(true);
        [SerializeField] private SfxReference _notEnoughSpiritPoints = new SfxReference(true);

        public void OnSpendSpiritPoint() {
            _spendSpiritPoint.Play();
        }

        public void OnNoSpiritPointsLeft()
        {
            _noSpiritPointsLeft.Play();
        }

        public void OnInteractionWindowOpen()
        {
            _interactionWindowOpen.Play();

        }

        public void OnNotEnoughSpiritPoints()
        {
            _notEnoughSpiritPoints.Play();
        }

        #endregion

        #region Main Menu

        [Header("Main Menu")]
        [SerializeField] private SfxReference _startGame = new SfxReference(true);
        [SerializeField] private SfxReference _loadGame = new SfxReference(true);
        [SerializeField] private SfxReference _quitGame = new SfxReference(true);
        [SerializeField] private SfxReference _saveGame = new SfxReference(true);

        public void OnStartGame() {
            _startGame.Play();
        }

        public void OnLoadGame()
        {
            _loadGame.Play();
        }

        public void OnQuitGame()
        {
            _quitGame.Play();
        }

        public void OnSaveGame()
        {
            _saveGame.Play();
        }

        #endregion

        #region Journal

        [Header("Journal")]
        [SerializeField] private SfxReference _journalNotification = new SfxReference(true);
        [SerializeField] private SfxReference _openJournal = new SfxReference(true);
        [SerializeField] private SfxReference _closeJournal = new SfxReference(true);
        [SerializeField] private SfxReference _switchPageLeft = new SfxReference(true);
        [SerializeField] private SfxReference _switchPageRight = new SfxReference(true);
        [SerializeField] private SfxReference _clickTab = new SfxReference(true);

        public void OnJournalNotification() {
            _journalNotification.Play();
        }

        public void OnOpenJournal() {
            _openJournal.Play();
        }

        public void OnCloseJournal() {
            _closeJournal.Play();
        }

        public void OnSwitchPageLeft() {
            _switchPageLeft.Play();
        }

        public void OnSwitchPageRight() {
            _switchPageRight.Play();
        }

        public void OnClickTab() {
            _clickTab.Play();
        }

        #endregion

        #region Dialogue

        [Header("Dialogue")]
        [SerializeField] private SfxReference _dialogueWindowAppears = new SfxReference(true);
        [SerializeField] private SfxReference _nextDialogue = new SfxReference(true);
        [SerializeField] private SfxReference _dialogueWindowCloses = new SfxReference(true);

        public void OnDialogueWindowAppears() {
            _dialogueWindowAppears.Play();
        }

        public void OnNextDialogue() {
            _nextDialogue.Play();
        }

        public void OnDialogueWindowCloses() {
            _dialogueWindowCloses.Play();
        }

        #endregion
    }
}