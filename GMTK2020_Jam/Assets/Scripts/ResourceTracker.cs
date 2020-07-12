using System;
using UnityEngine.UI;
using System.Collections;
using UnityEngine;

/// <summary>
/// simple tracker for a resource while updating a fill image to represent it
/// </summary>
public class ResourceTracker : MonoBehaviour
{
    public static ResourceTracker instance;
    
    [SerializeField]
    private string _resourceName = "Will";
    [SerializeField]
    private float _value;
    [SerializeField]
    private float _valueMax;
    [SerializeField]
    private Image _meter;

    private void Awake()
    {
        instance = this;
    }

    private void Start() {
        if (_meter != null) UpdateMeter();
    }

    public void UpdateResource(float delta) { 
        _value += delta;
        _value = Mathf.Clamp(_value, 0.0f, _valueMax);
        if (_meter != null) UpdateMeter();
    }

    private void UpdateMeter() {
        _meter.fillAmount = _value / _valueMax;
    }


    public float GetValue()
    {
        return _value;
    }


    public bool TrySpendResource(float cost)
    {
        if (_value > cost)
        {
            UpdateResource(-cost);
            return true;
        }

        return false;
    }

}
