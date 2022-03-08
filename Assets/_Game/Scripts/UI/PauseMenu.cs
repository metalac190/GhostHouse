using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Singleton;

    private bool isPaused = false;
    private bool canPause = true;

    //Menu Panels
    [SerializeField] GameObject pauseMenu = null;
    [SerializeField] Page[] pages; //Only 1 page active at a time
    [SerializeField] Page activePage;
    [SerializeField] GameObject tabs;

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
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(isPaused);
            tabs.SetActive(isPaused);
        }
        //Time.timeScale = isPaused ? 0f : 1f;
    }

    public void PauseGame()
    {
        if (!canPause) return;
        isPaused = true;
        UpdatePaused();
    }

    public void ResumeGame()
    {
        isPaused = false;
        UpdatePaused();
    }

    public void PreventPausing(bool canPause)
    {
        this.canPause = canPause;
        UpdatePaused();
    }

    public void SetActivePage(GameObject page)
    {
        activePage = page.GetComponent<Page>();
    }
}