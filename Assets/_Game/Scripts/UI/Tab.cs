using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tab : MonoBehaviour
{
    GameObject owner;
    Image image;
    public int associatedPage;
    Journal journal;
    Button tabBtn;

    private void Start()
    {
        journal = GameObject.Find("Journal Panel").GetComponent<Journal>();
        tabBtn = GetComponent<Button>();
        tabBtn.onClick.AddListener(OpenPage);
    }

    //Called when tab clicked
    public void OpenPage()
    {
        journal.ActivatePage(associatedPage);

        //Move tab to left side when this happens
       
    }

    //Reset tab position
    public void Reset()
    {

    }
}
