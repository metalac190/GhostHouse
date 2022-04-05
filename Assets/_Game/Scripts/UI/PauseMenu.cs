using Mechanics.Feedback;
using UnityEngine;
using Utility.Audio.Managers;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Singleton;

    private bool isPaused = false;
    private bool canPause = true;

    //Menu Panels
    [SerializeField] JournalController journal = null;
    [SerializeField] SfxUiLibrary sfxUiLibrary = null;

    private void Awake()
    {
        Singleton = this;
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
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
        if (isPaused) {
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
        IsometricCameraController.Singleton.gamePaused = isPaused;
        if (journal != null)
        {
            journal.gameObject.SetActive(isPaused);
        }
        //Time.timeScale = isPaused ? 0f : 1f;
    }

    public void PauseGame()
    {
        if (!canPause || isPaused) return;
        isPaused = true;
        sfxUiLibrary.OnOpenJournal();
        UpdatePaused();
    }

    public void ResumeGame()
    {
        if (!isPaused) return;
        if (journal.ClosePage()) {
            isPaused = false;
            sfxUiLibrary.OnCloseJournal();
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