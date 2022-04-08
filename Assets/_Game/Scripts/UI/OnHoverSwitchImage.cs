using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnHoverSwitchImage : MonoBehaviour
{
    public Sprite regularImage;
    public Sprite hoverImage;
    Image image;
    

    void Start()
    {
        image = GetComponent<Image>();
    }


    public void OnHover()
    {
        image.sprite = hoverImage;
    }

    public void OnExit()
    {
        image.sprite = regularImage;
    }
}
