using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] public int _startingSpiritPoints;
    [SerializeField] public Integer _currentSpiritPoints;

    private void Awake()
    {
        _currentSpiritPoints.value = _startingSpiritPoints;
    }

    public int GetCurrentSpiritPoints()
    {
        return _currentSpiritPoints.value;
    }
    public int GetStartingSpiritPoints()
    {
        return _startingSpiritPoints;
    }

    public void AddSpiritPoints(int num)
    {
        _currentSpiritPoints.value += num;
    }
    public void SubtractSpiritPoints(int num)
    {
        _currentSpiritPoints.value -= num;
    }
    public void SetSpiritPoints(int num)
    {
        _currentSpiritPoints.value = num;
    }

    

}
