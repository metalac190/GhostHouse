using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Utility.Buttons;

public class DataManager : MonoBehaviour
{
    // public instance reference to this script
    public static DataManager Instance = null;

    // string to hold the path to the savefile
    private string filePath;

    // rweference to SaveData object, will hold values to save
    private SaveData saveData = new SaveData();

    // Save Data fields saved in DataManager
    public int level { get; set; }
    public int remainingSpiritPoints { get; set; }

    [HideInInspector]
    public Dictionary<string, bool> interactions;

    public float settingsVolume { get; set; }
    public int settingsGraphics { get; set; }

    [HideInInspector]
    public bool[] journalUnlocks;

    private void Awake()
    {
        // Singleton pattern, there should only be one of these on the DataManager Prefab
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            filePath = Path.Combine(Application.persistentDataPath, "savedata.json");
            journalUnlocks = new bool[10];
            interactions = new Dictionary<string, bool>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {

    }

    // Read data from the save file into the game
    public void ReadFile()
    {
        if(File.Exists(filePath))
        {
            string fileContents = File.ReadAllText(filePath);
            JsonUtility.FromJsonOverwrite(fileContents, saveData);

            level = saveData.level;
            remainingSpiritPoints = saveData.remainingSpiritPoints;
            //saveData.interactionStates.CopyTo(interactableObjects, 0);
            settingsVolume = saveData.settings.volume;
            settingsGraphics = saveData.settings.graphics;
            saveData.journalUnlocks.CopyTo(journalUnlocks, 0);
        }
        else
        {
            Debug.Log("No save file exists");
        }
    }

    // Write the data into the save file
    public void WriteFile()
    {
        saveData.level = level;
        saveData.remainingSpiritPoints = remainingSpiritPoints;
        //interactableObjects.CopyTo(saveData.interactionStates, 0);
        saveData.settings.volume = settingsVolume;
        saveData.settings.graphics = settingsGraphics;
        journalUnlocks.CopyTo(saveData.journalUnlocks, 0);

        string jsonString = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(filePath, jsonString);
    }

    public void SetInteraction(string name, bool interacted)
    {
        interactions[name] = interacted;
    }

    public bool GetInteraction(string name)
    {
        if(interactions.ContainsKey(name))
        {
            return interactions[name];
        }
        else
        {
            return false;
        }
    }

    // Dump all data to the console
    public void DumpData()
    {
        string outstr = "Data Dump";
        outstr += "\nLevel: " + level.ToString();
        outstr += "\nSpirit Points: " + remainingSpiritPoints.ToString();
        outstr += "\nInteractables:";
        //for(int i = 0; i < interactions.Length; i++)
        //{
            //outstr += "\n\tInteractable " + i.ToString() + ": " + interactableObjects[i];
        //}
        outstr += "\nSettings:";
        outstr += "\n\tVolume: " + settingsVolume.ToString();
        outstr += "\n\tGraphics: " + settingsGraphics.ToString();
        outstr += "\nJournal Unlocks: ";
        for(int i = 0; i < journalUnlocks.Length; i++)
        {
            if(journalUnlocks[i])
            {
                outstr += i.ToString() + " ";
            }
        }
        Debug.Log(outstr);
    }

    // Dump save file contents to the console
    public void DumpFileContents()
    {
        Debug.Log(File.ReadAllText(filePath));
    }

    // Mark a journal entry as unlocked
    public void UnlockJournalEntry(int index)
    {
        try
        {
            journalUnlocks[index] = true;
        }
        catch(System.Exception ex)
        {
            Debug.Log("journal entry failed at: " + index.ToString());
        }
    }

    [Button(Mode = ButtonMode.NotPlaying)]
    public void ResetData()
    {
        interactions.Clear();
    }
}