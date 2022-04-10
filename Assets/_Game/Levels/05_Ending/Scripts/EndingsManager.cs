using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingsManager : MonoBehaviour
{
    [SerializeField]
    Ending trueEnding;

    [SerializeField]
    Ending cousinEnding;

    [SerializeField]
    Ending sisterEnding;

    [SerializeField]
    [Tooltip("Threshold for this ending is ignored.")]
    Ending badEnding;

    [SerializeField]
    Game.TransitionManager transitionManager = null;
    

    void Start()
    {
        DataManager data = DataManager.Instance;
        Ending selectedEnding;

        if (data.trueEndingPoints > -1)//trueEnding.Threshold)
        {
            selectedEnding = trueEnding;
        }
        else if (data.cousinsEndingPoints > cousinEnding.Threshold)
        {
            selectedEnding = cousinEnding;
        }
        else if (data.sistersEndingPoints > sisterEnding.Threshold)
        {
            selectedEnding = sisterEnding;
        }
        else
        {
            selectedEnding = badEnding;
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