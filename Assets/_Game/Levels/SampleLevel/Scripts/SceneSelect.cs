using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSelect : MonoBehaviour
{
    [SerializeField] GameObject _seasonPanel = null;

    private void Start()
    {
        _seasonPanel.SetActive(false);
    }
    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.name == "GrandfatherClock")
                {
                    _seasonPanel.SetActive(true);
                }
            }
        }
    }
}
