using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance = null;

    private string saveFile;

    public SaveData saveData = new SaveData();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            //savefile = Application.persistentDataPath + "/savedata.json";
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void ReadFile()
    {
        if(File.Exists(saveFile))
        {
            string fileContents = File.ReadAllText(saveFile);
            JsonUtility.FromJsonOverwrite(fileContents, saveData);
        }
    }

    public void WriteFile()
    {
        string jsonString = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(saveFile, jsonString);
    }
}
