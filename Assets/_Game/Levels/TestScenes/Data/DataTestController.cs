﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTestController : MonoBehaviour
{
    int level;
    int sp;
 
    // Start is called before the first frame update
    void Start()
    {
        level = 0;
        sp = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            DataManager.Instance.DumpData();
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            DataManager.Instance.DumpFileContents();
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            DataManager.Instance.WriteFile();
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            DataManager.Instance.ReadFile();
        }
    }

    public void IncreaseLevel()
    {
        level++;
        DataManager.Instance.level = level;
        DataManager.Instance.UnlockJournalEntry(level);
    }

    public void DecreaseSP()
    {
        sp--;
        DataManager.Instance.remainingSpiritPoints = sp;
    }
}
