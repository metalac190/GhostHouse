using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    [System.Serializable]
    public class Interaction
    {
        public bool interacted = false;
    }

    [System.Serializable]
    public class Dialogue
    {

    }

    [System.Serializable]
    public struct Settings
    {
        public float volume;
        public int graphics;
    }

    public List<Interaction> interactions;
    public List<Dialogue> dialogues;

    public bool[] journalUnlocks;
}
