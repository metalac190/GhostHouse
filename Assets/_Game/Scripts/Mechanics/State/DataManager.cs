using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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
    public ScriptableObject[] interactableObjects;

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
            Debug.Log(filePath);
        }
        else
        {
            Destroy(this.gameObject);
        }
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
            saveData.interactionStates.CopyTo(interactableObjects, 0);
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
        interactableObjects.CopyTo(saveData.interactionStates, 0);
        saveData.settings.volume = settingsVolume;
        saveData.settings.graphics = settingsGraphics;
        journalUnlocks.CopyTo(saveData.journalUnlocks, 0);

        string jsonString = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(filePath, jsonString);
    }

    public ScriptableObject GetInteractableState(int index)
    {
        return interactableObjects[index];
    }

    public void SetInteractableState(int index, string state)
    {
        //interactableObjects[index] = state;
    }

    public void DumpData()
    {
        string outstr = "Data Dump";
        outstr += "\nLevel: " + level.ToString();
        outstr += "\nSpirit Points: " + remainingSpiritPoints.ToString();
        outstr += "\nInteractables:";
        for(int i = 0; i < interactableObjects.Length; i++)
        {
            outstr += "\n\tInteractable " + i.ToString() + ": " + interactableObjects[i];
        }
        outstr += "\nSettings:";
        outstr += "\n\tVolume: " + settingsVolume.ToString();
        outstr += "\n\tGraphics: " + settingsGraphics.ToString();
        outstr += "Journal Unlocks: ";
        for(int i = 0; i < journalUnlocks.Length; i++)
        {
            if(journalUnlocks[i])
            {
                outstr += i.ToString() + " ";
            }
        }
        Debug.Log(outstr);
    }

    public void DumpFileContents()
    {
        Debug.Log(File.ReadAllText(filePath));
    }
}