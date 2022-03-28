using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerHUD : MonoBehaviour
    {
        [Header("Spirit Lamp")]
        [SerializeField] private Image _lamp = null;
        [SerializeField] private Sprite _lampOn = null;
        [SerializeField] private Sprite _lampOff = null;

        [Header("Spirit Points")]
        [SerializeField] private List<Image> _spiritPoints = new List<Image>();
        [SerializeField] private Sprite _spiritPointBright = null;
        [SerializeField] private Sprite _spiritPointDull = null;

        private int _maxPoints;

        private void Awake() {
            _spiritPoints = _spiritPoints.Where(image => image != null).ToList();
        }

        private void Start() {
            SetMaxSpiritPoints(DataManager.Instance.remainingSpiritPoints);
        }

        // Call this on Start() to setup spirit points
        public void SetMaxSpiritPoints(int maxPoints) {
            //Debug.Log(maxPoints);
            if (_spiritPoints.Count < maxPoints) {
                Debug.LogError("Not enough Spirit Points in List to sustain a max of " + maxPoints, gameObject);
                return;
            }
            for (var i = 0; i < _spiritPoints.Count; i++) {
                _spiritPoints[i].gameObject.SetActive(i < maxPoints);
                _spiritPoints[i].sprite = _spiritPointBright;
            }
            _maxPoints = maxPoints;
        }

        public void UpdateSpiritPoints(int aboutToSpend = 0) {
            SetSpiritPoints(DataManager.Instance.remainingSpiritPoints, aboutToSpend);
        }

        // Call this each time the number of spirit points changes
        public void SetSpiritPoints(int points, int aboutToSpend = 0) {
            //Debug.Log("Spirit Points: " + points + ". About to Spend " + aboutToSpend + ".");
            for (var i = 0; i < _maxPoints; i++) {
                if (i >= points) {
                    _spiritPoints[i].enabled = false;
                }
                else if (i >= points - aboutToSpend) {
                    _spiritPoints[i].enabled = true;
                    _spiritPoints[i].sprite = _spiritPointDull;
                }
                else {
                    _spiritPoints[i].enabled = true;
                    _spiritPoints[i].sprite = _spiritPointBright;
                }
            }
            _lamp.sprite = points + aboutToSpend > 0 ? _lampOn : _lampOff;
        }

        public void OpenJournal() {
            PauseMenu.Singleton.PauseGame();
        }
    }
}