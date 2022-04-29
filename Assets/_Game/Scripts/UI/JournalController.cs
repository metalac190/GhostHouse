using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility.Audio.Helper;
using Utility.ReadOnly;

public class JournalController : MonoBehaviour
{
    [SerializeField] private TabSwitcher _tabSwitcher = null;
    [SerializeField] private Animator _journalAnimator = null;
    [SerializeField] private float _journalCloseTime = 0.1f;

    [Header("Main Pages")]
    [SerializeField] private List<GameObject> _settingsPages = new List<GameObject>();
    [SerializeField] private PanelMediator _settingsController = null;
    [SerializeField] private List<GameObject> _pausePages = new List<GameObject>();
    [SerializeField] private GameObject _quitConfirmation = null;
    [SerializeField] private bool _hideButtonsOnPausePage = true;

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
    [SerializeField, ReadOnly] private int _currentPageNum;

    [Header("Sfx")]
    [SerializeField] private SfxReference _journalOpen = new SfxReference(true);
    [SerializeField] private SfxReference _journalClose = new SfxReference(true);
    [SerializeField] private SfxReference _journalPageLeft = new SfxReference(true);
    [SerializeField] private SfxReference _journalPageRight = new SfxReference(true);

    private Coroutine _closeRoutine;

    public void OpenSettings(int pageNum = 0) => OpenPage(PageEnum.Settings, pageNum);
    public void OpenPauseMenu(int pageNum = 0) => OpenPage(PageEnum.PauseMenu, pageNum);
    public void OpenSpring(int pageNum = 0) => OpenPage(PageEnum.Spring, pageNum);
    public void OpenSummer(int pageNum = 0) => OpenPage(PageEnum.Summer, pageNum);
    public void OpenFall(int pageNum = 0) => OpenPage(PageEnum.Fall, pageNum);
    public void OpenWinter(int pageNum = 0) => OpenPage(PageEnum.Winter, pageNum);
    public void OpenCharacters(int pageNum = 0) => OpenPage(PageEnum.Characters, pageNum);
    public void OpenEndings(int pageNum = 0) => OpenPage(PageEnum.Endings, pageNum);

    private void SetJournalAnim(string anim) {
        if (_journalAnimator == null) return;
        _journalAnimator.ResetTrigger("open");
        _journalAnimator.ResetTrigger("close");
        _journalAnimator.SetTrigger(anim);
    }

    public void OpenJournal() {
        _journalOpen.Play();
        SetJournalAnim("open");
        if (_closeRoutine != null) StopCoroutine(_closeRoutine);
        OpenPage(PageEnum.PauseMenu);
    }

    public bool OpenPage(PageEnum pageEnum, int pageNum = 0) {
        var pageList = GetPageList(pageEnum);
        if (!CheckPageValid(pageList, pageNum)) return false;
        if (pageEnum == _currentPage && pageNum == _currentPageNum) {
            return NextPageWithinSection();
        }

        bool pageRight = 10 * (int)pageEnum + pageNum > 10 * (int)_currentPage + _currentPageNum;
        if (pageRight) {
            _journalPageLeft.Play();
        }
        else {
            _journalPageRight.Play();
        }

        CloseAll();
        pageList[pageNum].SetActive(true);
        _currentPage = pageEnum;
        _currentPageNum = pageNum;

        if (pageEnum == PageEnum.Settings && _settingsController != null) {
            _settingsController.UpdateSettings();
        }

        SetNavigationButtons();
        if (_tabSwitcher != null) _tabSwitcher.SetPage(_currentPage);
        return true;
    }

    public void NextPage() {
        if (NextPageWithinSection()) {
            return;
        }
        var nextPage = GetNextPage(_currentPage);
        if (nextPage == _currentPage) {
            // No next page exists
            return;
        }
        OpenPage(nextPage);
    }

    public bool NextPageWithinSection() {
        int pageNum = _currentPageNum + 1;
        return OpenPage(_currentPage, pageNum);
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
        if (_quitConfirmation != null && _quitConfirmation.activeSelf) {
            _quitConfirmation.SetActive(false);
            return false;
        }
        _journalClose.Play();
        SetJournalAnim("close");
        _closeRoutine = StartCoroutine(CloseJournal());
        return true;
    }

    private IEnumerator CloseJournal() {
        for (float t = 0; t < _journalCloseTime; t += Time.deltaTime) {
            yield return null;
        }
        _closeRoutine = null;
        gameObject.SetActive(false);
    }

    private void SetNavigationButtons() {
        bool pageCheck = !(_hideButtonsOnPausePage && _currentPage == PageEnum.PauseMenu);
        bool firstCheck = !(_currentPage == PageEnum.Settings && _currentPageNum == 0);
        _previousButton.gameObject.SetActive(pageCheck && firstCheck);
        bool lastCheck = !(_currentPage == PageEnum.Endings && _currentPageNum >= GetPageList(_currentPage).Count - 1);
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