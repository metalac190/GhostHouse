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
    RectTransform rect;
    Vector3 originalPos;

    private void Start()
    {
        journal = GameObject.Find("Journal Panel").GetComponent<Journal>();
        tabBtn = GetComponent<Button>();
        tabBtn.onClick.AddListener(OpenPage);
        rect = GetComponent<RectTransform>();   
        originalPos = rect.anchoredPosition;
    }

    //Called when tab clicked
    public void OpenPage()
    {
        journal.ActivatePage(associatedPage);

        //Move appropiate tabs when this happens
        journal.UpdateTabs();
    }

    public void ChangePosition(bool isActive)
    {
        rect.localScale = new Vector3(1, 1, 1);

        if (isActive)
        {
            rect.anchoredPosition = new Vector3(-635, originalPos.y, 0);
            return;
        }

        rect.anchoredPosition = new Vector3(-655, originalPos.y, 0);
    }

    //Reset tab position
    public void ResetPosition()
    {
        rect.anchoredPosition = originalPos;
        rect.localScale = new Vector3(-1, 1, 1);
    }
}
