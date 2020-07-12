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
    [SerializeField]
    private float _flashTimer = 0.65f; //how long to flash for
    [SerializeField]
    private float _flashSpeed = 5.0f; 
    [SerializeField]
    private Color _colorToFlash = Color.gray;
    [SerializeField]
    private Image _meterBackground;

    public bool regenActive = true;
    public float regenRate = 0.8f;

    private Coroutine _flashRoutine = null;

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

    public void TestResourceSpend(float cost) {
        TrySpendResource(cost);
    }

    public bool TrySpendResource(float cost)
    {
        if (_value > cost)
        {
            UpdateResource(-cost);
            return true;
        }
        if(_flashRoutine == null && _meterBackground) _flashRoutine = StartCoroutine("FlashBar");
        return false;
    }

    private IEnumerator FlashBar() {
        float timer = 0.0f;
        float step = 360.0f / _flashTimer;
        float currentStep = 0.0f;
        Vector3 original = new Vector3(_meterBackground.color.r, _meterBackground.color.g, _meterBackground.color.b);
        Vector3 target = new Vector3(_colorToFlash.r, _colorToFlash.g, _colorToFlash.b);
        Vector3 newColor = Vector3.zero;

        while(timer <= _flashTimer) {
            newColor = Vector3.Lerp(original, target, Mathf.Sin(currentStep / 2.0f));
            _meterBackground.color = new Color(newColor.x, newColor.y, newColor.z);
            currentStep += step * Time.deltaTime * _flashSpeed;
            timer += Time.deltaTime;
            yield return 0.0f;
        }

        _meterBackground.color = new Color(original.x, original.y, original.z);
        _flashRoutine = null;
        yield return 0.0f;
    }
}
