using System;
using UnityEngine;

namespace Utility.RangedFloats
{
    [Serializable]
    public struct RangedFloat
    {
        public float MinValue;
        public float MaxValue;

        public RangedFloat(float value) {
            MinValue = value;
            MaxValue = value;
        }

        public RangedFloat(float min, float max) {
            MinValue = min;
            MaxValue = max;
        }

        public float GetRandom() {
            return UnityEngine.Random.Range(MinValue, MaxValue);
        }

        public float Clamp(float value) {
            return Mathf.Clamp(value, MinValue, MaxValue);
        }
    }
}