using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class StickyNote : MonoBehaviour
{
    public bool isInteractable = true;
    
    [Space(5)]
    [SerializeField]
    private GameObject _textCanvas;
    [SerializeField]
    private Renderer _renderer;
    [SerializeField]
    private Material[] _materials;
    [SerializeField]
    private Color _outlineColor = Color.red;

    public float thickness = 0.2f;

    [Space(5)]
    public AudioSource audioSource;
    public AudioClip[] onHoverSounds;
    public AudioClip onInteractSuccessSound;
    public AudioClip onInteractFailSound;

    public UnityEvent onInteractSuccessUnityAction;
    public UnityEvent onInteractFailUnityAction;

    private Color prevColor;
    
    protected virtual void Start() 
    {
        _textCanvas.SetActive(false);
        
        if (_renderer == null)
        {
            _renderer = GetComponent<Renderer>();
        }
        if (_renderer != null)
        {
            if (_renderer.GetType() == (typeof(MeshRenderer)))
            {
                _renderer.material = _materials[0];
            }
            
            if(_renderer.GetType() == (typeof(SpriteRenderer)))
            {
                prevColor = ((SpriteRenderer) _renderer).color;
            }
        }
    }
    public void OnMouseEnter() {
        if(!isInteractable)
            return;
        //Mouse Entered
        _textCanvas.SetActive(true);

        if (_renderer != null)
        {
            if (_renderer.GetType() == (typeof(MeshRenderer)))
            {
                ((MeshRenderer)_renderer).material = _materials[1];
                ((MeshRenderer)_renderer).material.SetColor("_OutlineColor", _outlineColor);
                ((MeshRenderer)_renderer).material.SetColor("_Color", Color.white);
                ((MeshRenderer)_renderer).material.SetFloat("_Outline", thickness);
            }
            
            if (_renderer.GetType() == (typeof(SpriteRenderer)))
            {
                ((SpriteRenderer) _renderer).color = _outlineColor;
            }
            
        }

        if (audioSource != null && onHoverSounds.Length > 0)
        {
            audioSource.PlayOneShot(onHoverSounds[Random.Range(0, onHoverSounds.Length)]);
        }
    }

    public void OnMouseExit() {
        //Mouse Left
        
        _textCanvas.SetActive(false);
        
        if (_renderer != null)
        {
            if (_renderer.GetType() == (typeof(MeshRenderer)))
            {
                _renderer.material = _materials[0];
            }
            if (_renderer != null && _renderer.GetType() == (typeof(SpriteRenderer)))
            {
                ((SpriteRenderer) _renderer).color = prevColor;
            }
        }

    }

    public void OnMouseOver() {
        //_textBox.gameObject.SetActive(true);
    }

    public void OnMouseDown() 
    {
        if(!isInteractable)
            return;
        
        Debug.Log("Clicked on " + name);

//        if (ResourceTracker.instance.TrySpendResource(interactCost))
//        {
//            OnInteractSuccess();
//        }
//        else
//        {
//            OnInteractFailed();
//        }
    }
}
