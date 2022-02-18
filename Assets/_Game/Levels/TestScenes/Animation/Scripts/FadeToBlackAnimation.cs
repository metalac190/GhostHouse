using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeToBlackAnimation : MonoBehaviour
{
    //
    // Current event is on Test_Interaction. 
    // It is brute forced so definitely change it
    //

    [Header("Animation Stats")]
    [SerializeField] float _fadeSpeed = 1f;
    [SerializeField] float _timeBlack = 1f;

    [Header("UI Panel")]
    [SerializeField] GameObject _blackOutPanel = null;
    Image _blackOutImage;

    // for brute force testing
    // change as needed
    [SerializeField] Test_Interaction _interaction = null;

    private void Awake()
    {
        _blackOutPanel.SetActive(true);
        _blackOutImage = _blackOutPanel.GetComponent<Image>();
        _blackOutPanel.SetActive(false);

        // for brute force testing
        // change as needed
        _interaction.InvokeAction += StartFadeToBlack;

    }

    public void StartFadeToBlack()
    {
        _blackOutPanel.SetActive(true);
        StartCoroutine(FadeToBlack());
    }

    IEnumerator FadeToBlack(bool fadeToBlack = true)
    {
        Color objectColor = _blackOutImage.color;

        float fadeAmount;

        if (fadeToBlack)
        {
            while (_blackOutImage.color.a <= 1)
            {
                fadeAmount = objectColor.a + (_fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                _blackOutImage.color = objectColor;
                yield return null;
            }

            yield return new WaitForSeconds(_timeBlack);

            while (_blackOutImage.color.a >= 0)
            {
                fadeAmount = objectColor.a - (_fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                _blackOutImage.color = objectColor;
                yield return null;
            }
        }
    

        yield return new WaitForEndOfFrame();
        _blackOutPanel.SetActive(false);
    }
}
