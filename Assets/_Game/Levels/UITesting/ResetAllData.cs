using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAllData : MonoBehaviour
{
    public void ResetGameData()
    {
        DataManager.Instance.ResetAllData();
    }
}
