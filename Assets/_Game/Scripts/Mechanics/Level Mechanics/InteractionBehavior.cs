using System.Collections.Generic;
using System.Linq;
using Mechanics.Feedback;
using Mechanics.UI;
using UnityEngine;
using Utility.Audio.Managers;

public class InteractionBehavior : InteractableBase
{
    /*This is the main Interaction Behavior Script that determines what an Interactable object does when interacted with.*/

    #region Variables
    [Header("Sfx On Click")]
    [SerializeField] private bool _sfxOnClick = false;
    [SerializeField] private SfxType _sfx = SfxType.None;

    [Header("Text On Hover")]
    [SerializeField] private bool _textOnHover = false;
    [SerializeField] private bool _useObjectName = false;
    [SerializeField] private string _hoverText = "";

    [Header("Material Override On Hover")]
    [SerializeField] private bool _setMaterialOnHover = false;
    [SerializeField] private Material _materialToSet = null;

    private List<MeshRenderer> _meshRenderers;
    private List<Material> _baseMaterial;

    private bool _missingHoverUi;

    private bool OverrideText => _textOnHover && (_useObjectName || !string.IsNullOrEmpty(_hoverText));
    private bool OverrideMaterial => _setMaterialOnHover && _materialToSet != null;

    #endregion

    private void Start()
    {
        if (TextHoverController.Singleton == null)
        {
            _missingHoverUi = true;
            Debug.LogWarning("Missing Text Hover Controller in Scene!");
        }
    }

    #region Feedback Methods

    //nothing here for right now

    #endregion

    #region Hovering Methods
    public override void OnHoverEnter()
    {
        Debug.Log("Started Hovering Over: " + gameObject.name);
        //When the mouse starts hovering over the object. Runs only once.

        //Brandon's Story Interactable Code:
        if (_missingHoverUi) return;
        if (OverrideText)
        {
            string text = _useObjectName ? name : _hoverText;
            TextHoverController.Singleton.StartHover(text);
        }
        if (OverrideMaterial)
        {
            if (_meshRenderers == null)
            {
                _meshRenderers = gameObject.GetComponentsInChildren<MeshRenderer>().ToList();
            }
            _baseMaterial = new List<Material>();
            foreach (var meshRenderer in _meshRenderers)
            {
                _baseMaterial.Add(meshRenderer.material);
                meshRenderer.material = _materialToSet;
            }
        }
    }

    public override void OnHoverExit()
    {
        Debug.Log("Stopped Hovering Over: " + gameObject.name);
        //When the mouse stops hovering over the object. Runs only once.

        //Brandon's Story Interactable Code:
        if (_missingHoverUi) return;
        if (OverrideText)
        {
            TextHoverController.Singleton.EndHover();
        }
        if (OverrideMaterial)
        {
            for (var i = 0; i < _meshRenderers.Count; i++)
            {
                _meshRenderers[i].material = _baseMaterial[i];
            }
            _baseMaterial.Clear();
        }
    }

    #endregion

    #region Clicking Methods
    public override void OnLeftClick()
    {
        Debug.Log("Left Clicked On: " + gameObject.name);
        //When the mouse clicks on the object. Left Mouse button cannot be held down as this only runs once per click.

        //Brandon's Story Interactable Code:
        if (_sfxOnClick)
        {
            SoundManager.Instance.PlaySfx(_sfx);
        }
    }

    public override void OnLeftClick(Vector3 mousePosition)
    {
        //Same thing as the function above, but this one also contains the mouse's position when it clicks.
    }

    public override void OnRightClick()
    {
        Debug.Log("Right Clicked On: " + gameObject.name);
        //When the mouse right clicks on the object. Right Mouse button cannot be held down as this only runs once per click.

        //Brandon's Story Interactable Code:
        if (_sfxOnClick)
        {
            SoundManager.Instance.PlaySfx(_sfx);
        }
    }

    public override void OnRightClick(Vector3 mousePosition)
    {
        //Same thing as the function above, but this one also contains the mouse's position when it clicks.
    }

    #endregion
}
