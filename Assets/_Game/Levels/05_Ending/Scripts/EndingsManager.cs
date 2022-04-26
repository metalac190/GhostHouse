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
        List<EndingPair> possibleChoices = new List<EndingPair>();

        // get prioritized list of endings not yet unlocked
        if (!data.endingUnlocks[0] && data.trueEndingPoints >= _trueEnding.Threshold)
        {
            possibleChoices.Add(new EndingPair(_trueEnding, 0));
        }
        else if (!data.endingUnlocks[3] && data.sistersEndingPoints >= _sisterEnding.Threshold)
        {
            possibleChoices.Add(new EndingPair(_sisterEnding, 3));
        }
        else if (!data.endingUnlocks[2] && data.cousinsEndingPoints >= _cousinEnding.Threshold)
        {
            possibleChoices.Add(new EndingPair(_cousinEnding, 2));
        }

        EndingPair selectedEnding;
        // choose from list of endings not done yet, if possible
        if (possibleChoices.Count > 0)
        {
            selectedEnding = possibleChoices[0];
        }
        // follow default priorities
        else
        {
            if (!data.endingUnlocks[0] && data.trueEndingPoints >= _trueEnding.Threshold)
            {
                selectedEnding = new EndingPair(_trueEnding, 0);
            }
            else if (!data.endingUnlocks[3] && data.sistersEndingPoints >= _sisterEnding.Threshold)
            {
                selectedEnding = new EndingPair(_sisterEnding, 3);
            }
            else if (!data.endingUnlocks[2] && data.cousinsEndingPoints >= _cousinEnding.Threshold)
            {
                selectedEnding = new EndingPair(_cousinEnding, 2);
            }
            else
            {
                selectedEnding = new EndingPair(badEnding, 1);
            }
        }

        foreach (Ending end in new List<Ending>() { _trueEnding, _cousinEnding, _sisterEnding, badEnding })
        {
            if (end == selectedEnding.ending)
            {
                end.Visuals?.SetActive(true);
                _transitionManager._interactionOnStart = end.Dialog;
                _musicManager.PlayMusic(end.MusicTrack);
                DataManager.Instance.endingUnlocks[selectedEnding.index] = true;
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
    class Ending
    {
        [Min(0)]
        public int Threshold = 0;

        public Interactable Dialog = null;
        public GameObject Visuals = null;
        public MusicTrack MusicTrack = null;
    }

    class EndingPair
    {
        public Ending ending;
        public int index;

        public EndingPair(Ending ending, int index)
        {
            this.ending = ending;
            this.index = index;
        }
    }
}