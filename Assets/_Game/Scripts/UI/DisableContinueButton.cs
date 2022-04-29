
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableContinueButton : MonoBehaviour
{
    [SerializeField] private Button _button = null;

    private void Start() {
        var savedLevel = DataManager.Instance.level;
        bool buttonAvailable = savedLevel == "Summer" || savedLevel == "Fall" || savedLevel == "Winter";
        if (_button != null) _button.interactable = buttonAvailable;
    }

    public void Disable()
    {
        var savedLevel = DataManager.Instance.level;
        bool buttonAvailable = savedLevel == "Summer" || savedLevel == "Fall" || savedLevel == "Winter";
        if (_button != null) _button.interactable = buttonAvailable;
    }
}
