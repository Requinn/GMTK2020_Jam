using TMPro;
using UnityEngine;

/// <summary>
/// Object in scene that use can mouse over and click on
/// </summary>
public class InteractableObject : MonoBehaviour
{

    public float interactCost = 0f;
    public bool isInteractable = true;
    
    [Space(5)]
    [SerializeField]
    private GameObject _textCanvas;
    [SerializeField]
    private MeshRenderer _renderer;
    [SerializeField]
    private Material[] _materials;
    [SerializeField]
    private Color _outlineColor;

    public float thickness = 1f;

    [Space(5)]
    public AudioSource audioSource;
    public AudioClip[] onHoverSounds;
    public AudioClip onInteractSuccessSound;
    public AudioClip onInteractFailSound;
    
    

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
        if(!isInteractable)
            return;
        //Mouse Entered
        _textCanvas.SetActive(true);
        _renderer.material = _materials[1];
        _renderer.material.SetColor("_OutlineColor", _outlineColor);
        _renderer.material.SetColor("_Color", Color.white);
        _renderer.material.SetFloat("_Outline", thickness);

        if (audioSource != null && onHoverSounds.Length > 0)
        {
            audioSource.PlayOneShot(onHoverSounds[Random.Range(0, onHoverSounds.Length)]);
        }
    }

    public void OnMouseExit() {
        //Mouse Left
        _textCanvas.SetActive(false);
        _renderer.material = _materials[0];
    }

    public void OnMouseOver() {
        //_textBox.gameObject.SetActive(true);
    }

    public void OnMouseDown() 
    {
        if(!isInteractable)
            return;
        
        Debug.Log("Clicked on " + name);

        if (ResourceTracker.instance.TrySpendResource(interactCost))
        {
            OnInteractSuccess();
        }
        else
        {
            
            OnInteractFailed();
        }
        
    }


    protected virtual void OnInteractSuccess()
    {
        Debug.Log(name + ": Click Success");
        if(audioSource && onInteractSuccessSound)
            audioSource.PlayOneShot(onInteractSuccessSound);
    }
    
    protected virtual void OnInteractFailed()
    {
        Debug.Log(name + ":Click Fail");
        if(audioSource && onInteractFailSound)
            audioSource.PlayOneShot(onInteractFailSound);
    }
    
}
