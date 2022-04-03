using System;
using System.Collections;
using System.Collections.Generic;
using Mechanics.Feedback;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility.Audio.Managers;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Singleton;

    private bool isPaused = false;
    private bool canPause = true;

    //Menu Panels
    [SerializeField] GameObject pauseMenu = null;
    [SerializeField] Page[] pages; //Only 1 page active at a time
    [SerializeField] Page activePage;
    [SerializeField] GameObject tabs = null;

    [SerializeField] private SfxUiLibrary _sfxUiLibrary = null;

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
    }

    private void UpdatePaused()
    {
        ModalWindowController.Singleton.HideHudOnPause(isPaused);
        SoundManager.MusicManager.SetPaused(isPaused);
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(isPaused);
            tabs.SetActive(isPaused);
        }
        //Time.timeScale = isPaused ? 0f : 1f;
    }

    public void PauseGame()
    {
        if (!canPause || isPaused) return;
        isPaused = true;
        _sfxUiLibrary.OnOpenJournal();
        UpdatePaused();
    }

    public void ResumeGame()
    {
        if (!isPaused) return;
        isPaused = false;
        _sfxUiLibrary.OnCloseJournal();
        UpdatePaused();
    }

    public void PreventPausing(bool updateCanPause)
    {
        // If no longer able to pause but also currently paused, resume
        if (!updateCanPause && isPaused) {
            ResumeGame();
        }
        canPause = updateCanPause;
    }

    public void SetActivePage(GameObject page)
    {
        activePage = page.GetComponent<Page>();
    }
}