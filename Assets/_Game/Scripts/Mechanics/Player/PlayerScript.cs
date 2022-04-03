using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] public int _startingSpiritPoints;
    [SerializeField] public int _currentSpiritPoints;

    private void Awake()
    {
        _currentSpiritPoints = 3;
    }

    public int GetCurrentSpiritPoints()
    {
        return _currentSpiritPoints;
    }
    public int GetStartingSpiritPoints()
    {
        return _startingSpiritPoints;
    }

    public void AddSpiritPoints(int num)
    {
        _currentSpiritPoints += num;
    }
    public void SubtractSpiritPoints(int num)
    {
        _currentSpiritPoints -= num;
    }
    public void SetSpiritPoints(int num)
    {
        _currentSpiritPoints = num;
    }

    

}
