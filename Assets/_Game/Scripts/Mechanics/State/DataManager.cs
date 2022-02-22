using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    // public instance reference to this script
    public static DataManager Instance = null;

    // string to hold the path to the savefile
    private string filePath;

    // Reference to SaveData object, will hold values to save
    private SaveData saveData = new SaveData();

    // Save Data fields saved in DataManager
    public int level { get; set; }
    public int remainingSpiritPoints { get; set; }

    [SerializeField] private YarnObject[] yarnObjects;
    [SerializeField] private string[] interactableStates;
    public OrderedDictionary interactablesMap = new OrderedDictionary();

    public float settingsVolume { get; set; }
    public int settingsGraphics { get; set; }

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

    private void Start()
    {
        if(yarnObjects.Length != interactableStates.Length)
        {
            Debug.Log("Each Yarn Object should have a state");
            for(int i = 0; i < yarnObjects.Length; i++)
            {
                interactablesMap.Add(yarnObjects[i], interactableStates[i]);
            }
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
            for(int i = 0; i < interactableStates.Length; i++)
            {
                interactablesMap[i] = saveData.interactionStates[i];
            }
            settingsVolume = saveData.settings.volume;
            settingsGraphics = saveData.settings.graphics;
            for(int i = 0; i < journalUnlocks.Length; i++)
            {
                journalUnlocks[i] = saveData.journalUnlocks[i];
            }
        }
    }

    // Write the data into the save file
    public void WriteFile()
    {
        saveData.level = level;
        saveData.remainingSpiritPoints = remainingSpiritPoints;
        for(int i = 0; i < interactableStates.Length; i++)
        {
            saveData.interactionStates[i] = (string)interactablesMap[i];
        }
        saveData.settings.volume = settingsVolume;
        saveData.settings.graphics = settingsGraphics;
        for(int i = 0; i < journalUnlocks.Length; i++)
        {
            saveData.journalUnlocks[i] = journalUnlocks[i];
        }

        string jsonString = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(filePath, jsonString);
    }

    public string GetInteractableState(YarnObject yo)
    {
        return (string)interactablesMap[yo];
    }

    public void SetInteractableState(YarnObject yo, string state)
    {
        interactablesMap[yo] = state;
    }
}
