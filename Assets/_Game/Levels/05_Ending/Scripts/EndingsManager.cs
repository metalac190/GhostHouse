using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingsManager : MonoBehaviour
{
    [SerializeField]
    Ending trueEnding = null;

    [SerializeField]
    Ending cousinEnding = null;

    [SerializeField]
    Ending sisterEnding = null;

    [SerializeField]
    [Tooltip("Threshold for this ending is ignored.")]
    Ending badEnding = null;

    [SerializeField]
    Game.TransitionManager transitionManager = null;
    

    void Start()
    {
        DataManager data = DataManager.Instance;
        Ending selectedEnding;

        if (data.trueEndingPoints > trueEnding.Threshold)
        {
            selectedEnding = trueEnding;
            data.UnlockEnding(0);
        }
        else if (data.sistersEndingPoints > sisterEnding.Threshold)
        {
            selectedEnding = sisterEnding;
            data.UnlockEnding(3);
        }
        else if (data.cousinsEndingPoints > cousinEnding.Threshold)
        {
            selectedEnding = cousinEnding;
            data.UnlockEnding(2);
        }
        else
        {
            selectedEnding = badEnding;
            data.UnlockEnding(1);
        }

        foreach (Ending end in new List<Ending>() { trueEnding, cousinEnding, sisterEnding, badEnding })
        {
            if (end == selectedEnding)
            {
                end.Visuals?.SetActive(true);
                transitionManager._dialogueOnStart = end.Dialog;
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

        public string Dialog = string.Empty;
        public GameObject Visuals = null;

        public Ending(int threshold, string dialog, GameObject visuals)
        {
            Threshold = threshold;
            Dialog = dialog;
            Visuals = visuals;
        }
    }
}