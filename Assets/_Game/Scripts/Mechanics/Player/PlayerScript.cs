using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //Variables
    string _playerName = "testName";

    //Accessor "Getter" Methods
    public string GetPlayerName()
    {
        return _playerName;
    }

    //Mutator "Setter" Methods
    public void SetPlayerName(string _newName)
    {
        _newName = _playerName;
    }
}
