using UnityEditor;
using UnityEngine;

namespace Utilities
{
    public struct Condition
    {
        private enum ConditionType
        {
            Health,
            Bool,
            Other
        }

        public bool HealthType;
        public bool BoolType;

        [ConditionalHide("BoolType", true)] public CustomBool BoolName;
        [ConditionalHide("HealthType", true)] public float HealthTreshold;

    }

    public class CustomEvent
    {
        
    }

    public abstract class CustomBool
    {
        private bool _active;

        public bool GetActive()
        {
            return _active;
        }

        public void SetActive(bool state)
        {
            _active = state;
        }
    }
}