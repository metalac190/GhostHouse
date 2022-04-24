using Game;
using Mechanics.Level_Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility.Audio.Clips;
using Utility.Audio.Managers;

public class EndingsManager : MonoBehaviour
{
    [SerializeField]
    Ending _trueEnding = null;

    [SerializeField]
    Ending _cousinEnding = null;

    [SerializeField]
    Ending _sisterEnding = null;

    [SerializeField]
    [Tooltip("Threshold for this ending is ignored.")]
    Ending badEnding = null;

    [Space]
    [SerializeField]
    Game.TransitionManager _transitionManager = null;

    [SerializeField]
    MusicManager _musicManager = null;
    

    void Start()
    {
        DataManager data = DataManager.Instance;
        Ending selectedEnding;

        if (data.trueEndingPoints >= _trueEnding.Threshold)
        {
            selectedEnding = _trueEnding;
            data.UnlockEnding(0);
        }
        else if (data.sistersEndingPoints >= _sisterEnding.Threshold)
        {
            selectedEnding = _sisterEnding;
            data.UnlockEnding(3);
        }
        else if (data.cousinsEndingPoints >= _cousinEnding.Threshold)
        {
            selectedEnding = _cousinEnding;
            data.UnlockEnding(2);
        }
        else
        {
            selectedEnding = badEnding;
            data.UnlockEnding(1);
        }

        foreach (Ending end in new List<Ending>() { _trueEnding, _cousinEnding, _sisterEnding, badEnding })
        {
            if (end == selectedEnding)
            {
                end.Visuals?.SetActive(true);
                _transitionManager._interactionOnStart = end.Dialog;
                _musicManager.PlayMusic(end.MusicTrack);
            }
            else
            {
                end.Visuals?.SetActive(false);
            }
        }
    }

    public void GoToScene(string nextScene)
    {
        SceneManager.LoadScene(nextScene);
    }

    [System.Serializable]
    private class Ending
    {
        [Min(0)]
        public int Threshold = 0;

        public Interactable Dialog = null;
        public GameObject Visuals = null;
        public MusicTrack MusicTrack = null;
    }
}