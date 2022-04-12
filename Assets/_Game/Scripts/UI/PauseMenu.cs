using Mechanics.Feedback;
using UnityEngine;
using Utility.Audio.Managers;
using Yarn.Unity;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Singleton;

    private bool isPaused = false;
    private bool canPause = true;

    //Menu Panels
    [SerializeField] JournalController journal = null;

    private DialogueRunner dialogueRunner;

    private void Awake()
    {
        Singleton = this;
        dialogueRunner = FindObjectOfType<DialogueRunner>();
    }

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        UpdatePaused();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (dialogueRunner != null && dialogueRunner.IsDialogueRunning && !isPaused) {
                return;
            }
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        } else if (isPaused) {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                journal.PreviousPage();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                journal.NextPage();
            }
        }
    }

    private void UpdatePaused()
    {
        ModalWindowController.Singleton.HideHudOnPause(isPaused);
        SoundManager.MusicManager.SetPaused(isPaused);
        if (canPause) {
            IsometricCameraController.Singleton.gamePaused = isPaused;
        }
        if (journal != null)
        {
            journal.gameObject.SetActive(isPaused);
            if (isPaused) {
                // TODO: Open to Journal Notification!
                journal.OpenJournal();
            }
        }
        //Time.timeScale = isPaused ? 0f : 1f;
    }

    public void PauseGame()
    {
        if (!canPause || isPaused) return;
        isPaused = true;
        UpdatePaused();
    }

    public void ResumeGame()
    {
        if (!isPaused) return;
        if (journal.ClosePage()) {
            isPaused = false;
            UpdatePaused();
        }
    }

    public void PreventPausing(bool updateCanPause)
    {
        // If no longer able to pause but also currently paused, resume
        if (!updateCanPause && isPaused) {
            ResumeGame();
        }
        canPause = updateCanPause;
    }
}