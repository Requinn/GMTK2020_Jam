using TMPro;
using UnityEngine;

/// <summary>
/// Object in scene that use can mouse over and click on
/// </summary>
public class InteractableObject : MonoBehaviour
{
    [SerializeField]
    private GameObject _textCanvas;
    [SerializeField]
    private MeshRenderer _renderer;
    [SerializeField]
    private Material[] _materials;
    [SerializeField]
    private Color _outlineColor;

    public float thickness = 1f;

    private void Start() {
        if (_renderer == null)
        {
            _renderer = GetComponent<MeshRenderer>();
        }
        _textCanvas.SetActive(false);
        _renderer.material = _materials[0];
        
        _textCanvas.SetActive(false);
    }
    public void OnMouseEnter() {
        //Mouse Entered
        _textCanvas.SetActive(true);
        _renderer.material = _materials[1];
        _renderer.material.SetColor("_OutlineColor", _outlineColor);
        _renderer.material.SetColor("_Color", Color.white);
        _renderer.material.SetFloat("_Outline", thickness);
    }

    public void OnMouseExit() {
        //Mouse Left
        _textCanvas.SetActive(false);
        _renderer.material = _materials[0];
    }

    public void OnMouseOver() {
        //_textBox.gameObject.SetActive(true);
    }

    public void OnMouseDown() {
        Debug.Log("Clicked on " + name);
    }
}
