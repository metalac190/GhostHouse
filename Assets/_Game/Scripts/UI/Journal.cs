using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Journal : MonoBehaviour
{
    public Tab[] tabs = new Tab[7];

    public List<Page> pages = new List<Page>();

    [SerializeField]
    Page activePage, pausePage, lastPage;

    public Button nextBtn, previousBtn;

    private void Start()
    {
        tabs[0].associatedPage = 0;
        tabs[1].associatedPage = 1;
        tabs[2].associatedPage = 2; 
        tabs[3].associatedPage = 3;
        tabs[4].associatedPage = 4;
        tabs[5].associatedPage = 5;
        tabs[6].associatedPage = 7;

        //Gives each page a list identifier
        int i = 0;
        foreach (Page page in pages)
        {
            page.index = i;
            i++;
        }
        ActivatePage(pausePage);
    }

    public void NextPage()
    {
        int currentIndex = activePage.index;
        ActivatePage(currentIndex + 1);
    }

    public void PreviousPage()
    {
        int currentIndex = activePage.index;
        ActivatePage(currentIndex - 1);
    }

    public void ActivatePage(int pageNum)
    {
        if (activePage != null)
            activePage.gameObject.GetComponent<Renderer>().enabled = false;

        activePage = pages[pageNum];
        activePage.gameObject.GetComponent<Renderer>().enabled = true;
        UpdateTabs();
    }

    public void ActivatePage(Page page)
    {
        if (activePage != null)
            activePage.gameObject.GetComponent<Renderer>().enabled = false;

        activePage = page;
        activePage.gameObject.GetComponent<Renderer>().enabled = true;
        UpdateTabs();
    }

    void OnEnable()
    {
        ActivatePage(pausePage);
    }

    void UpdateTabs()
    {
        foreach (Tab tab in tabs)
        {
            Page tabPage = pages[tab.associatedPage];
            if (activePage.index < tabPage.index)
            {
                tab.Reset();
            }
        }
    }

    void Update()
    {
        if (activePage == pausePage)
        {
            previousBtn.gameObject.SetActive(false);
        }
        else if (activePage == lastPage)
        {
            nextBtn.gameObject.SetActive(false);
        }
        else
        {
            nextBtn.gameObject.SetActive(true);
            previousBtn.gameObject.SetActive(true);
        }
    }
}
