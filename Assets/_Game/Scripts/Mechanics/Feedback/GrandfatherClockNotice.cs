﻿using UnityEngine;
using Utility.Audio.Controllers;
using Utility.Buttons;
using Yarn.Unity;

public class GrandfatherClockNotice : MonoBehaviour
{
    [SerializeField] private GameObject _noticeSound = null;

    private DialogueRunner _dialogueRunner;
    private bool _check = true;

    private void Awake() {
        _noticeSound.SetActive(false);
    }

    private void Start() {
        _dialogueRunner = FindObjectOfType<DialogueRunner>();
    }

    public void Update() {
        if (_check && DataManager.Instance.remainingSpiritPoints == 0 && !_dialogueRunner.IsDialogueRunning) {
            _check = false;
            SpentAllPointsNotice();
        }
    }

    [Button(Mode = ButtonMode.NotPlaying)]
    public void SpentAllPointsNotice() {
        _noticeSound.SetActive(true);
    }
}
