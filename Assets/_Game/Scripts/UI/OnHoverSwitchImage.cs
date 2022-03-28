using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnHoverSwitchImage : MonoBehaviour
{
    public Sprite regularImage;
    public Sprite hoverImage;
    Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }

    void OnMouseEnter()
    {
        image.sprite = hoverImage;
    }

    void OnMouseExit()
    {
        image.sprite = regularImage;
    }
}
