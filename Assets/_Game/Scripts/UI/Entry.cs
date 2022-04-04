using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Entry : MonoBehaviour
{
    public bool isLocked = true;
    public GameObject owner;
    Image image = null;
    TextMeshPro text = null;

    void Start()
    {
        image = owner.GetComponent<Image>();
        text = owner.GetComponent<TextMeshPro>();

        if (isLocked)
        {
            text.text = "???????";

            //Encrypt image
            image.color = new Color(1, 1, 1, 0.75f);
        }
    }
}