using Mechanics.Feedback;
using UnityEngine;
using Utility.Audio.Managers;
using Yarn.Unity;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Singleton;
    public static System.Action<bool> PauseUpdated;

    public bool IsPaused { get; private set; } = false;

    private bool canPause = true;

    //Menu Panels
    [SerializeField] JournalController journal = null;

    private void Awake()
    {
        Singleton = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        IsPaused = false;
        UpdatePaused();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) {
            if (IsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        } else if (IsPaused) {
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
        ModalWindowController.Singleton.HideHudOnPause(IsPaused);
        SoundManager.MusicManager.SetPaused(IsPaused);
        if (canPause) {
            IsometricCameraController.Singleton.gamePaused = IsPaused;
        }
        if (journal != null)
        {
            journal.gameObject.SetActive(IsPaused);
            if (IsPaused) {
                // TODO: Open to Journal Notification!
                journal.OpenJournal();
            }
        }
        PauseUpdated?.Invoke(IsPaused);
    }

    public void PauseGame()
    {
        if (!canPause || IsPaused) return;
        IsPaused = true;
        UpdatePaused();
    }

    public void ResumeGame()
    {
        if (!IsPaused) return;
        if (journal.ClosePage()) {
            IsPaused = false;
            UpdatePaused();
        }
    }

    public void PreventPausing(bool updateCanPause)
    {
        // If no longer able to pause but also currently paused, resume
        if (!updateCanPause && IsPaused) {
            ResumeGame();
        }
        canPause = updateCanPause;
    }
}