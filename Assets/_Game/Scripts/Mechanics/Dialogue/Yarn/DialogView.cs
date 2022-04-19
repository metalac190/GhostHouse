using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Mechanics.Dialog
{
    public class DialogView : MonoBehaviour
    {
        public TextMeshProUGUI Txt_dialog => _txt_dialog;
        public Image Img_dialog => _img_dialog;
        public Image Img_portrait => _img_portrait;
        public GameObject P_characterName => _p_characterName;
        public TextMeshProUGUI Txt_characterName => _txt_characterName;
        public GameObject Btn_continue => _btn_continue;
        public Slider Sldr_progressbar => _sldr_progressbar;

        public Sprite DefaultBoxSprite => _defaultBoxSprite;
        public Color DefaultBoxColor => _defaultBoxColor;

        [SerializeField] TextMeshProUGUI _txt_dialog = null;
        [SerializeField] Image _img_dialog = null;
        [SerializeField] Image _img_portrait = null;
        [SerializeField] GameObject _p_characterName = null;
        [SerializeField] TextMeshProUGUI _txt_characterName = null;
        [SerializeField] GameObject _btn_continue = null;
        [SerializeField] Slider _sldr_progressbar = null;

        Sprite _defaultBoxSprite = null;
        Color _defaultBoxColor = Color.white;

        void Awake()
        {
            if (_img_dialog != null)
            {
                _defaultBoxSprite = _img_dialog.sprite;
                _defaultBoxColor = _img_dialog.color;
            }
        }
    }
}