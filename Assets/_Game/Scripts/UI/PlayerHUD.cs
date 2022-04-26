using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerHUD : MonoBehaviour
    {
        [Header("Spirit Lamp")]
        [SerializeField] private GameObject _lampOn = null;
        [SerializeField] private GameObject _lampOff = null;

        [Header("Spirit Points")]
        [SerializeField] private List<Animator> _spiritPoints = new List<Animator>();

        [Header("Journal Icon")]
        [SerializeField] private GameObject _journalNormal = null;
        [SerializeField] private GameObject _journalNotification = null;

        private int _maxPoints;

        private static string SetBright => "add SP";
        private static string SetDull => "setAboutToSpend";
        private static string SetOff => "lose SP";

        private static int SpiritPointsStart => DataManager.Instance.remainingSpiritPoints;
        private static int SpiritPointsCurrent => DataManager.Instance.remainingSpiritPoints;

        private void Awake() {
            _spiritPoints = _spiritPoints.Where(image => image != null).ToList();
        }

        private void Start() {
            SetMaxSpiritPoints(SpiritPointsStart);
            SetJournalNotification(false);
            _lampOn.gameObject.SetActive(true);
            _lampOff.gameObject.SetActive(false);
        }

        public void TestMaxSpiritPoints(int newMax) {
            if (newMax > _maxPoints) {
                SetMaxSpiritPoints(newMax);
            }
        }

        // Call this on Start() to setup spirit points
        public void SetMaxSpiritPoints(int maxPoints) {
            //Debug.Log(maxPoints);
            if (_spiritPoints.Count < maxPoints) {
                Debug.LogError("Not enough Spirit Points in List to sustain a max of " + maxPoints, gameObject);
                return;
            }
            for (var i = 0; i < _spiritPoints.Count; i++) {
                bool active = i < maxPoints;
                _spiritPoints[i].gameObject.SetActive(active);
                if (active) {
                    _spiritPoints[i].SetTrigger(SetBright);
                }
            }
            _maxPoints = maxPoints;
        }

        public void UpdateSpiritPoints(int aboutToSpend = 0) {
            SetSpiritPoints(SpiritPointsCurrent, aboutToSpend);
        }

        // Call this each time the number of spirit points changes
        public void SetSpiritPoints(int points, int aboutToSpend = 0) {
            //Debug.Log("Spirit Points: " + points + ". About to Spend " + aboutToSpend + ".");
            for (var i = 0; i < _maxPoints; i++) {
                if (i >= points) {
                    _spiritPoints[i].ResetTrigger(SetBright);
                    _spiritPoints[i].ResetTrigger(SetDull);
                    _spiritPoints[i].SetTrigger(SetOff);
                }
                else if (i >= points - aboutToSpend) {
                    _spiritPoints[i].ResetTrigger(SetBright);
                    _spiritPoints[i].ResetTrigger(SetOff);
                    _spiritPoints[i].SetTrigger(SetDull);
                }
                else {
                    _spiritPoints[i].gameObject.SetActive(true);
                    _spiritPoints[i].ResetTrigger(SetOff);
                    _spiritPoints[i].ResetTrigger(SetDull);
                    _spiritPoints[i].SetTrigger(SetBright);
                }
            }
            if (points + aboutToSpend == 0) {
                _lampOn.gameObject.SetActive(false);
                _lampOff.gameObject.SetActive(true);
            }
            else if (_lampOff.activeSelf) {
                _lampOn.gameObject.SetActive(true);
                _lampOff.gameObject.SetActive(false);
            }
        }

        public void AddJournalNotification() {
            SetJournalNotification(true);
            // TODO: Add functionality for which page to open?
        }

        public void ClearJournalNotification() {
            SetJournalNotification(false);
        }

        private void SetJournalNotification(bool notification) {
            _journalNormal.SetActive(!notification);
            _journalNotification.SetActive(notification);
        }

        public void OpenJournal() {
            PauseMenu.Singleton.PauseGame();
        }
    }
}