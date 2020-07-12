using System;
using UnityEngine.UI;
using System.Collections;
using TMPro;
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
    [SerializeField]
    private TextMeshProUGUI _meterText;

    public bool regenActive = true;
    public float regenRate = 0.8f;
    

    private void Awake()
    {
        instance = this;
    }

    private void Start() {
        if (_meter != null) UpdateMeter();
    }

    private void Update()
    {
        UpdateResource(Time.deltaTime * regenRate);
    }

    public void UpdateResource(float delta) { 
        _value += delta;
        _value = Mathf.Clamp(_value, 0.0f, _valueMax);
        if (_meter != null) UpdateMeter();

        if (_meterText)
        {
            _meterText.text = string.Format("Willpower ({0})", (int)_value);
        }
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
