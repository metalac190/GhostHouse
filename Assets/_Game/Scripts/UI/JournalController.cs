using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility.ReadOnly;

public class JournalController : MonoBehaviour
{
    [SerializeField] private TabSwitcher _tabSwitcher = null;

    [Header("Main Pages")]
    [SerializeField] private List<GameObject> _settingsPages = new List<GameObject>();
    [SerializeField] private List<GameObject> _pausePages = new List<GameObject>();
    [SerializeField] private GameObject _quitConfirmation = null;

    [Header("Seasonal Pages")]
    [SerializeField] private List<GameObject> _springPages = new List<GameObject>();
    [SerializeField] private List<GameObject> _summerPages = new List<GameObject>();
    [SerializeField] private List<GameObject> _fallPages = new List<GameObject>();
    [SerializeField] private List<GameObject> _winterPages = new List<GameObject>();

    [Header("Other Pages")]
    [SerializeField] private List<GameObject> _charactersPages = new List<GameObject>();
    [SerializeField] private List<GameObject> _endingsPages = new List<GameObject>();

    [Header("Navigation Buttons")]
    [SerializeField] private Button _previousButton = null;
    [SerializeField] private Button _nextButton = null;

    [Header("Current")]
    [SerializeField, ReadOnly] private PageEnum _currentPage = PageEnum.PauseMenu;
    [SerializeField, ReadOnly] private int _currentPageNum = 0;

    public void OpenSettings(int pageNum = 0) => OpenPage(PageEnum.Settings, pageNum);
    public void OpenPauseMenu(int pageNum = 0) => OpenPage(PageEnum.PauseMenu, pageNum);
    public void OpenSpring(int pageNum = 0) => OpenPage(PageEnum.Spring, pageNum);
    public void OpenSummer(int pageNum = 0) => OpenPage(PageEnum.Summer, pageNum);
    public void OpenFall(int pageNum = 0) => OpenPage(PageEnum.Fall, pageNum);
    public void OpenWinter(int pageNum = 0) => OpenPage(PageEnum.Winter, pageNum);
    public void OpenCharacters(int pageNum = 0) => OpenPage(PageEnum.Characters, pageNum);
    public void OpenEndings(int pageNum = 0) => OpenPage(PageEnum.Endings, pageNum);

    public bool OpenPage(PageEnum pageEnum, int pageNum = 0) {
        var pageList = GetPageList(pageEnum);
        if (!CheckPageValid(pageList, pageNum)) return false;
        if (pageEnum == _currentPage && pageNum == _currentPageNum) {
            return NextPage(false);
        }

        CloseAll();
        pageList[pageNum].SetActive(true);
        _currentPage = pageEnum;
        _currentPageNum = pageNum;

        SetNavigationButtons();
        if (_tabSwitcher != null) _tabSwitcher.SetPage(_currentPage);
        return true;
    }

    public void NextPage() => NextPage(true);
    public bool NextPage(bool canUseNextSection) {
        int pageNum = _currentPageNum + 1;
        if (OpenPage(_currentPage, pageNum)) {
            return true;
        }
        if (!canUseNextSection) {
            return false;
        }
        var nextPage = GetNextPage(_currentPage);
        if (nextPage == _currentPage) {
            // No next page exists
            return false;
        }
        return OpenPage(nextPage);
    }

    public void PreviousPage() {
        var page = _currentPage;
        int pageNum = _currentPageNum - 1;
        if (pageNum < 0) {
            page = GetPreviousPage(page);
            if (page == _currentPage) {
                // No previous page exists
                return;
            }
            pageNum = GetPageList(page).Count - 1;
        }
        OpenPage(page, pageNum);
    }

    // Returns true if Journal should close
    public bool ClosePage() {
        if (_currentPage == PageEnum.PauseMenu) {
            if (_quitConfirmation != null && _quitConfirmation.activeSelf) {
                _quitConfirmation.SetActive(false);
                return false;
            }
            return true;
        }
        OpenPauseMenu();
        return false;
    }

    private void SetNavigationButtons() {
        bool pageCheck = !(_currentPage == PageEnum.Settings || _currentPage == PageEnum.PauseMenu);
        _previousButton.gameObject.SetActive(pageCheck);
        bool lastCheck = !(_currentPage == PageEnum.Endings && _currentPageNum < GetPageList(_currentPage).Count);
        _nextButton.gameObject.SetActive(pageCheck && lastCheck);
    }

    private List<GameObject> GetPageList(PageEnum pageEnum) {
        switch (pageEnum) {
            case PageEnum.Settings:
                return _settingsPages;
            case PageEnum.PauseMenu:
                return _pausePages;
            case PageEnum.Spring:
                return _springPages;
            case PageEnum.Summer:
                return _summerPages;
            case PageEnum.Fall:
                return _fallPages;
            case PageEnum.Winter:
                return _winterPages;
            case PageEnum.Characters:
                return _charactersPages;
            case PageEnum.Endings:
                return _endingsPages;
            default:
                return _settingsPages;
        }
    }

    private static bool CheckPageValid(IReadOnlyList<GameObject> pageList, int pageNum) {
        if (pageNum < 0) return false;
        if (pageNum >= pageList.Count) return false;
        return pageList[pageNum] != null;
    }

    private void CloseAll() {
        foreach (var page in _settingsPages) {
            page.SetActive(false);
        }
        foreach (var page in _pausePages) {
            page.SetActive(false);
        }
        foreach (var page in _springPages) {
            page.SetActive(false);
        }
        foreach (var page in _summerPages) {
            page.SetActive(false);
        }
        foreach (var page in _fallPages) {
            page.SetActive(false);
        }
        foreach (var page in _winterPages) {
            page.SetActive(false);
        }
        foreach (var page in _charactersPages) {
            page.SetActive(false);
        }
        foreach (var page in _endingsPages) {
            page.SetActive(false);
        }
    }

    private PageEnum GetNextPage(PageEnum currentPage) {
        switch (currentPage) {
            case PageEnum.Settings:
                return PageEnum.PauseMenu;
            case PageEnum.PauseMenu:
                return PageEnum.Spring;
            case PageEnum.Spring:
                return PageEnum.Summer;
            case PageEnum.Summer:
                return PageEnum.Fall;
            case PageEnum.Fall:
                return PageEnum.Winter;
            case PageEnum.Winter:
                return PageEnum.Characters;
            case PageEnum.Characters:
                return PageEnum.Endings;
            case PageEnum.Endings:
                return PageEnum.Endings;
            default:
                return PageEnum.PauseMenu;
        }
    }

    private PageEnum GetPreviousPage(PageEnum currentPage) {
        switch (currentPage) {
            case PageEnum.Settings:
                return PageEnum.Settings;
            case PageEnum.PauseMenu:
                return PageEnum.Settings;
            case PageEnum.Spring:
                return PageEnum.PauseMenu;
            case PageEnum.Summer:
                return PageEnum.Spring;
            case PageEnum.Fall:
                return PageEnum.Summer;
            case PageEnum.Winter:
                return PageEnum.Fall;
            case PageEnum.Characters:
                return PageEnum.Winter;
            case PageEnum.Endings:
                return PageEnum.Characters;
            default:
                return PageEnum.PauseMenu;
        }
    }
}