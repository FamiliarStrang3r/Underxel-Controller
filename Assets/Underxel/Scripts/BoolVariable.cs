using System;
using UnityEngine;

[Serializable]
public struct BoolVariable
{
    [SerializeField] private bool variableValue;
    public event Action<bool> OnValueChanged;

    public bool Value
    {
        get => variableValue;
        set
        {
            if (value != variableValue)
            {
                variableValue = value;
                OnValueChanged?.Invoke(variableValue);
            }
        }
    }
}
