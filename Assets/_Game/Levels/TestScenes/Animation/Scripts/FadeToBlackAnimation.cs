using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeToBlackAnimation : MonoBehaviour
{
    /*
     * Current event is on Test_Interaction. 
     * It is brute forced so definitely change it
    */

    [Header("Animation Stats")]
    [SerializeField] float _fadeSpeed = 1f;
    [SerializeField] float _timeBlack = 1f;

    [Header("UI Panel")]
    [SerializeField] GameObject _blackOutPanel = null;
    Image _blackOutImage;

    // for brute force testing
    // change as needed
    [SerializeField] Test_Interaction _interaction = null;

    //possible changes for events
    /*
     * [SerializeField] ScriptName or GameObject _eventCaller
    */

    private void Awake()
    {
        // not necessary, just there to demonstrate we do not need the panel or canvas active the whole time
        // cannot deactivate this canvas since the script is on it, but we can layout the real canvas to be different

        _blackOutPanel.SetActive(true);
        _blackOutImage = _blackOutPanel.GetComponent<Image>();
        _blackOutPanel.SetActive(false);

        // for brute force testing
        // change as needed

        /* call event here
        _interaction = GetComponent < "Where the event is" > ();
        */
        
        _interaction.InvokeAction += StartFadeToBlack;

    }

    public void StartFadeToBlack()
    {

        // only necessary if calling from action event

        _blackOutPanel.SetActive(true);
        StartCoroutine(FadeToBlack());
    }

    IEnumerator FadeToBlack(bool fadeToBlack = true)
    {
        Color objectColor = _blackOutImage.color;

        float fadeAmount;

        if (fadeToBlack)
        {

            // alpha channel is set to 0 by default
            // this increases the alpha by the rate set in the inspector

            while (_blackOutImage.color.a <= 1)
            {
                fadeAmount = objectColor.a + (_fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                _blackOutImage.color = objectColor;
                yield return null;
            }

            // waits while black for the duration set in the inspector

            yield return new WaitForSeconds(_timeBlack);

            // returns the alpha channel to 0 by the same speed
            
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
