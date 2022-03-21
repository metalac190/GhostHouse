#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using Utility.RangedFloats;

[CustomPropertyDrawer(typeof(RangedFloat), true)]
public class RangedFloatDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        label = EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, label);

        SerializedProperty minProp = property.FindPropertyRelative("MinValue");
        SerializedProperty maxProp = property.FindPropertyRelative("MaxValue");

        float minValue = Mathf.Round(minProp.floatValue * 10000) * 0.0001f;
        float maxValue = Mathf.Round(maxProp.floatValue * 10000) * 0.0001f;

        float rangeMin = 0;
        float rangeMax = 1;

        var ranges = (MinMaxRangeAttribute[])fieldInfo.GetCustomAttributes(typeof(MinMaxRangeAttribute), true);
        if (ranges.Length > 0) {
            rangeMin = ranges[0].Min;
            rangeMax = ranges[0].Max;
        }

        const float rangeBoundsLabelWidth = 56;
        const float gap = 4f;

        var rangeBoundsLabel1Rect = new Rect(position) { width = rangeBoundsLabelWidth - gap };
        //GUI.Label(rangeBoundsLabel1Rect, new GUIContent(minValue.ToString("F2")));
        position.xMin += rangeBoundsLabelWidth + gap;

        var rangeBoundsLabel2Rect = new Rect(position);
        rangeBoundsLabel2Rect.xMin = rangeBoundsLabel2Rect.xMax - rangeBoundsLabelWidth + gap;
        //GUI.Label(rangeBoundsLabel2Rect, new GUIContent(maxValue.ToString("F2")));
        position.xMax -= rangeBoundsLabelWidth + gap;


        EditorGUI.BeginChangeCheck();

        var minValue2 = EditorGUI.DelayedFloatField(rangeBoundsLabel1Rect, minValue);
        if (position.xMax > position.xMin + 10) {
            EditorGUI.MinMaxSlider(position, ref minValue, ref maxValue, rangeMin, rangeMax);
        }
        var maxValue2 = EditorGUI.DelayedFloatField(rangeBoundsLabel2Rect, maxValue);

        if (EditorGUI.EndChangeCheck()) {
            var actualMin = minProp.floatValue;
            var actualMax = maxProp.floatValue;
            float newMin = actualMin;
            float newMax = actualMax;

            // Min changed by slider
            if (CheckDifferent(actualMin, minValue)) {
                newMin = Mathf.Clamp(minValue, rangeMin, rangeMax);
                if (newMin > actualMax) newMax = newMin;
            }
            // Min changed by float field
            if (CheckDifferent(actualMin, minValue2)) {
                newMin = Mathf.Clamp(minValue2, rangeMin, rangeMax);
                if (newMin > actualMax) newMax = newMin;
            }

            // Max changed by slider
            if (CheckDifferent(actualMax, maxValue)) {
                newMax = Mathf.Clamp(maxValue, rangeMin, rangeMax);
                if (newMax < actualMin) newMin = newMax;
            }
            // Max changed by float field
            if (CheckDifferent(actualMax, maxValue2)) {
                newMax = Mathf.Clamp(maxValue2, rangeMin, rangeMax);
                if (newMax < actualMin) newMin = newMax;
            }

            minProp.floatValue = newMin;
            maxProp.floatValue = newMax;
        }

        EditorGUI.EndProperty();
    }

    private static bool CheckDifferent(float value1, float value2) {
        return Math.Abs(value1 - value2) > 0.00001f;
    }
}
#endif