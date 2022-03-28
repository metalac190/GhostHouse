using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Page : MonoBehaviour
{
    [SerializeField] 
    List<Entry> entries = new List<Entry>();

    public int index;

    Journal journal;

    private void Start()
    {
        journal = GameObject.Find("Journal Panel").GetComponent<Journal>();
    }
}
