using UnityEngine;
using Utility.Audio.Helper;

namespace Mechanics.Feedback
{
    [CreateAssetMenu(menuName = "Sound System/Libraries/Sfx UI Library")]
    public class SfxUiLibrary : ScriptableObject
    {
        [Header("Buttons")]
        [SerializeField] private SfxReference _hoverOverButton = new SfxReference(true);
        [SerializeField] private SfxReference _clickButtonGeneric = new SfxReference(true);

        public void OnButtonHover() => _hoverOverButton.Play();
        public void OnButtonClick() => _clickButtonGeneric.Play();

        [Header("Sliders")]
        [SerializeField] private SfxReference _startDraggingSlider = new SfxReference(true);
        [SerializeField] private SfxReference _dragSlider = new SfxReference(true);
        
        public void OnSliderClick() => _startDraggingSlider.Play();
        public void OnSliderDrag() => _dragSlider.Play();

        [Header("Spirit Points")]
        [SerializeField] private SfxReference _spendSpiritPoint = new SfxReference(true);
        [SerializeField] private SfxReference _noSpiritPointsLeft = new SfxReference(true);
        [SerializeField] private SfxReference _interactionWindowOpen = new SfxReference(true);
        [SerializeField] private SfxReference _interactionWindowClose = new SfxReference(true);

        public void OnSpendSpiritPoint() => _spendSpiritPoint.Play();
        public void OnNoSpiritPointsLeft() => _noSpiritPointsLeft.Play();
        public void OnInteractionWindowOpen() => _interactionWindowOpen.Play();
        public void OnInteractionWindowClose() => _interactionWindowClose.Play();

        [Header("Main Menu")]
        [SerializeField] private SfxReference _startGame = new SfxReference(true);
        [SerializeField] private SfxReference _loadGame = new SfxReference(true);
        [SerializeField] private SfxReference _quitGame = new SfxReference(true);
        [SerializeField] private SfxReference _saveGame = new SfxReference(true);

        public void OnStartGame() => _startGame.Play();
        public void OnLoadGame() => _loadGame.Play();
        public void OnQuitGame() => _quitGame.Play();
        public void OnSaveGame() => _saveGame.Play();

        [Header("Journal")]
        [SerializeField] private SfxReference _journalNotification = new SfxReference(true);
        [SerializeField] private SfxReference _openJournal = new SfxReference(true);
        [SerializeField] private SfxReference _closeJournal = new SfxReference(true);
        [SerializeField] private SfxReference _switchPageLeft = new SfxReference(true);
        [SerializeField] private SfxReference _switchPageRight = new SfxReference(true);
        [SerializeField] private SfxReference _hoverTab = new SfxReference(true);

        public void OnJournalNotification() => _journalNotification.Play();
        public void OnJournalOpen() => _openJournal.Play();
        public void OnJournalClose() => _closeJournal.Play();
        public void OnJournalPageLeft() => _switchPageLeft.Play();
        public void OnJournalPageRight() => _switchPageRight.Play();
        public void OnJournalHoverTab() => _hoverTab.Play();

        [Header("Dialogue")]
        [SerializeField] private SfxReference _dialogueWindowAppears = new SfxReference(true);
        [SerializeField] private SfxReference _nextDialogue = new SfxReference(true);
        [SerializeField] private SfxReference _dialogueWindowCloses = new SfxReference(true);

        public void OnDialogueWindowAppears() => _dialogueWindowAppears.Play();
        public void OnNextDialogue() => _nextDialogue.Play();
        public void OnDialogueWindowCloses() => _dialogueWindowCloses.Play();
    }
}