using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class RadialLayout : LayoutGroup
    {
        [SerializeField] private float _dist = 100;
        [SerializeField] private bool _offsetLast = true;
        [SerializeField] private bool _flipEveryOther = true;
        [SerializeField, Range(-360, 360)] private float _min = 0;
        [SerializeField, Range(-360, 360)] private float _max = 360;

        #region Layout Stuff

        protected override void OnEnable() {
            base.OnEnable();
            SetLayout();
        }

        public override void SetLayoutHorizontal() {
        }

        public override void SetLayoutVertical() {
        }

        public override void CalculateLayoutInputVertical() {
            SetLayout();
        }

        public override void CalculateLayoutInputHorizontal() {
            SetLayout();
        }

#if UNITY_EDITOR
        protected override void OnValidate() {
            base.OnValidate();
            SetLayout();
        }
#endif

        #endregion

        private void SetLayout() {
            m_Tracker.Clear();
            int count = transform.Cast<Transform>().Count(child => child.gameObject.activeSelf);
            if (count <= 0) return;

            float current = _min;
            float separation = (_max - _min) / (count + (_offsetLast ? 0 : -1));
            bool flip = false;
            foreach (RectTransform child in transform) {
                if (!child.gameObject.activeSelf) continue;
                // Layout stuff
                var props = DrivenTransformProperties.Anchors | DrivenTransformProperties.AnchoredPosition | DrivenTransformProperties.Pivot;
                m_Tracker.Add(this, child, props);
                // Set positions
                Vector3 angle = new Vector3(Mathf.Cos(current * Mathf.Deg2Rad), Mathf.Sin(current * Mathf.Deg2Rad), 0);
                child.localPosition = angle * _dist;
                current += separation;
                // Flip every other
                if (_flipEveryOther) {
                    flip = !flip;
                }
                child.localScale = new Vector3(flip ? 1 : -1, 1, 1);
                // Fix annoying centering
                var centerPos = new Vector2(0.5f, 0.5f);
                child.anchorMin = child.anchorMax = child.pivot = centerPos;
            }
        }
    }
}