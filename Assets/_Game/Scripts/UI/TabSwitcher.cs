using System;
using UnityEngine;

public class TabSwitcher : MonoBehaviour
{
    private void SetPauseMenu(bool left) => SetTab(_pauseMenuTabL, _pauseMenuTabR, left);
    [SerializeField] private GameObject _pauseMenuTabL = null;
    [SerializeField] private GameObject _pauseMenuTabR = null;

    private void SetSpring(bool left) => SetTab(_springTabL, _springTabR, left);
    [SerializeField] private GameObject _springTabL = null;
    [SerializeField] private GameObject _springTabR = null;

    private void SetSummer(bool left) => SetTab(_summerTabL, _summerTabR, left);
    [SerializeField] private GameObject _summerTabL = null;
    [SerializeField] private GameObject _summerTabR = null;

    private void SetFall(bool left) => SetTab(_fallTabL, _fallTabR, left);
    [SerializeField] private GameObject _fallTabL = null;
    [SerializeField] private GameObject _fallTabR = null;

    private void SetWinter(bool left) => SetTab(_winterTabL, _winterTabR, left);
    [SerializeField] private GameObject _winterTabL = null;
    [SerializeField] private GameObject _winterTabR = null;

    private void SetCharacter(bool left) => SetTab(_characterTabL, _characterTabR, left);
    [SerializeField] private GameObject _characterTabL = null;
    [SerializeField] private GameObject _characterTabR = null;

    private void SetEnding(bool left) => SetTab(_endingTabL, _endingTabR, left);
    [SerializeField] private GameObject _endingTabL = null;
    [SerializeField] private GameObject _endingTabR = null;

    public void SetPage(PageEnum page) {
        switch (page) {
            case PageEnum.Settings:
                SetAll(false);
                break;
            case PageEnum.PauseMenu:
                SetAll(false);
                SetPauseMenu(true);
                break;
            case PageEnum.Spring:
                SetAll(false);
                SetPauseMenu(true);
                SetSpring(true);
                break;
            case PageEnum.Summer:
                SetAll(false);
                SetPauseMenu(true);
                SetSpring(true);
                SetSummer(true);
                break;
            case PageEnum.Fall:
                SetAll(true);
                SetWinter(false);
                SetCharacter(false);
                SetEnding(false);
                break;
            case PageEnum.Winter:
                SetAll(true);
                SetCharacter(false);
                SetEnding(false);
                break;
            case PageEnum.Characters:
                SetAll(true);
                SetEnding(false);
                break;
            case PageEnum.Endings:
                SetAll(true);
                break;
        }
    }

    private void SetAll(bool left) {
        SetPauseMenu(left);
        SetSpring(left);
        SetSummer(left);
        SetFall(left);
        SetWinter(left);
        SetCharacter(left);
        SetEnding(left);
    }

    private void SetTab(GameObject left, GameObject right, bool leftActive) {
        left.SetActive(leftActive);
        right.SetActive(!leftActive);
    }
}
